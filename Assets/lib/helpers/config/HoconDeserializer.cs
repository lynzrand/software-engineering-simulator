using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Hocon;
// using Sesim.Helpers.Config.Deserializer;

namespace Sesim.Helpers.Config
{
    public static class HoconConfigDeserializer
    {
        private static Dictionary<string, HoconConfigCache> cache =
            new Dictionary<string, HoconConfigCache>();

        private static ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        private static Dictionary<Type, Converter<HoconValue, object>> deserializeFuncs =
            new Dictionary<Type, Converter<HoconValue, object>>();

        public static void ClearCache()
        {
            cache.Clear();
        }

        public static void CacheType(Type T)
        {
            _CacheType(T, isRoot: true);
        }

        internal static HoconConfigCache _CacheType(
            Type T,
            HashSet<HoconNodeAttribute> unresolvedNodes = null,
            HashSet<Type> unresolvedTypes = null,
            bool isRoot = false)
        {

            // Tries to get the specific attribute
            var attr = T.GetCustomAttribute(typeof(HoconConfigAttribute)) as HoconConfigAttribute;
            if (attr == null) throw new UnableToDeserializeHoconException();

            // try acquire lock
            cacheLock.EnterReadLock();
            if (cache.ContainsKey(attr.typeIdentifier)) return cache[attr.typeIdentifier];
            cacheLock.ExitReadLock();

            // initialize wipTypes
            unresolvedNodes = unresolvedNodes ?? new HashSet<HoconNodeAttribute>();
            unresolvedTypes = unresolvedTypes ?? new HashSet<Type>();

            if (!unresolvedTypes.Contains(T)) unresolvedTypes.Add(T);

            // Get all properties and fields to find those with HoconNodeAttribute
            var allProps = T.GetProperties();
            var allFields = T.GetType().GetFields();

            var nodes = new Dictionary<string, HoconNodeAttribute>();
            foreach (var prop in allProps)
            {
                // get prop attribute
                var pa = prop.GetCustomAttributes(typeof(HoconNodeAttribute)) as IEnumerable<HoconNodeAttribute>;
                var type = prop.DeclaringType;
                foreach (var a in pa)
                {
                    a.isProperty = true;
                    a.propertyInfo = prop;
                    resolveType(type, a, unresolvedNodes, unresolvedTypes);
                    if (a.key == null) a.key = prop.Name;
                    nodes.Add(a.key, a);
                }
            }

            foreach (var field in allFields)
            {
                // do the same stuff for fields
                var fa = field.GetCustomAttributes(typeof(HoconNodeAttribute)) as IEnumerable<HoconNodeAttribute>;
                var type = field.DeclaringType;
                foreach (var a in fa)
                {
                    a.isProperty = false;
                    a.fieldInfo = field;
                    resolveType(type, a, unresolvedNodes, unresolvedTypes);
                    if (a.key == null) a.key = field.Name;
                    nodes.Add(a.key, a);
                }
            }
            var configCache = new HoconConfigCache()
            {
                type = T,
                conf = attr,
                nodes = nodes,
                isDeserializerTrivial = true
            };

            // Waits so only one thread can cache the type
            cacheLock.EnterWriteLock();

            // If other threads have already added this class when we are processing,
            // just return the result.
            if (cache.ContainsKey(attr.typeIdentifier)) return configCache;

            cache.Add(attr.typeIdentifier, configCache);
            deserializeFuncs.Add(T, GetTrivialDeserializeFunc(T));

            unresolvedTypes.Remove(T);

            if (isRoot)
            {
                // We expect no unresolved types here; if there is any then there's a bug
                if (unresolvedTypes.Count > 0)
                {
                    throw new UnableToDeserializeHoconException($"Unable to cache types {unresolvedTypes}");
                }

                // assign deserialize functions to unresolved nodes
                foreach (var node in unresolvedNodes)
                {
                    if (deserializeFuncs.ContainsKey(node.Type))
                    {
                        node.converter = deserializeFuncs[node.Type];
                        unresolvedNodes.Remove(node);
                    }
                }

                // We also expect no unresolved node here; if there's any then it's also a bug
                if (unresolvedNodes.Count > 0)
                {
                    throw new UnableToDeserializeHoconException($"Unable to cache nodes {unresolvedNodes}");
                }

            }

            // Allow other threads to modify
            cacheLock.ExitWriteLock();
            return configCache;
        }

        private static void resolveType(
            Type T,
            HoconNodeAttribute a,
            HashSet<HoconNodeAttribute> unresolvedNodes,
            HashSet<Type> unresolvedTypes)
        {

            if (unresolvedTypes.Contains(T))
                unresolvedNodes.Add(a);
            else
            {
                var conf = _CacheType(T, unresolvedNodes, unresolvedTypes);
                a.converter = deserializeFuncs[a.Type];
            }

        }

        /// <summary>
        /// Gets the converter to convert <c>HoconValue</c> to <c>T</c>
        /// </summary>
        /// <param name="T">The type to get</param>
        /// <param name="unresolvedTypes">What types are not yet resolved (but will later)</param>
        /// <returns></returns>
        private static Converter<HoconValue, object> GetConverter(
            Type T,
            HashSet<Type> unresolvedTypes = null)
        {
            if (deserializeFuncs.ContainsKey(T))
            {
                // The simplest case: we already have this class's deserialize function
                return deserializeFuncs[T];
            }
            else if (isArray(T, out var rank, out var arrayInnerType))
            {
                // This class is an Array
                if (rank > 1)
                    throw new ArgumentException("Deserializing arrays with rank more than 1 is not supported");

                var innerConv = GetConverter(arrayInnerType);
                Converter<HoconValue, object> conv =
                    (HoconValue v) => v.GetArray().ConvertAll(innerConv).ToArray();

                AssignTypeConverter(T, conv);
                return conv;
            }
            else if (isICollection(T, out var collectionInnerType))
            {
                // This class is a collection (List, Set, etc.)
                var innerConv = GetConverter(collectionInnerType);

                Converter<HoconValue, object> conv =
                    (HoconValue v) =>
                    {
                        var arr = v.GetArray();
                        var t = T.GetConstructor(new Type[0]).Invoke(new object[0]);
                        foreach (var i in arr)
                            T.InvokeMember("Add", BindingFlags.Public | BindingFlags.InvokeMethod, null, t, new object[] { innerConv(i) });
                        return t;
                    };

                AssignTypeConverter(T, conv);
                return conv;
            }
            else if (isIDictionary(T, out var keyType, out var valueType))
            {
                // This class is a dictionary (Dictionary, etc.)
                if (keyType == typeof(string))
                {
                    var valueConv = GetConverter(valueType);
                    Converter<HoconValue, object> conv = (HoconValue v) =>
                    {
                        var dict = v.GetObject();
                        var t = T.GetConstructor(new Type[0]).Invoke(new object[0]);
                        foreach (var kvp in dict)
                        {
                            T.InvokeMember("Add", BindingFlags.Public | BindingFlags.InvokeMethod, null, t, new object[] { kvp.Key, valueConv(kvp.Value.Value) });
                        }
                        return t;
                    };
                    return conv;
                }
                else
                    throw new ArgumentException("Hocon Deserializer does not support converting dictionaries with non-string keys");
            }
            else if (typeof(IHoconDeserializable).IsAssignableFrom(T))
            {
                // This class has a custom deserialize method
                var methodInfo = T.GetRuntimeMethod(
                        "ReadFromHocon", new Type[] { typeof(HoconValue) });
                return (HoconValue v) => methodInfo.Invoke(
                    T.TypeInitializer.Invoke(new object[0]),
                    new object[] { v });
            }
            else
            {
                // This class is none of all above
                // Try all T's base types
                var baseType = T.BaseType;
                var baseTypeConverter = GetConverter(baseType);
                if (baseTypeConverter != null) return baseTypeConverter;

                // Try all T's implemented interfaces
                var interfaces = T.GetInterfaces();
                foreach (var i in interfaces)
                {
                    var interfaceConverter = GetConverter(i);
                    if (i != null) return interfaceConverter;
                }

                // Nothing works. Cache this type.
                if (unresolvedTypes?.Contains(T) ?? false)
                {
                    // This type is already to be cached
                    return GetTrivialDeserializeFunc(T);
                }
                else
                {
                    // This type is not cached but also not to be resolved
                    CacheType(T);
                    return GetTrivialDeserializeFunc(T);
                }
            }
        }

        public static void AssignTypeConverter(Type T, Converter<HoconValue, object> converter)
        {
            cacheLock.EnterWriteLock();
            deserializeFuncs.Add(T, converter);
            cacheLock.ExitWriteLock();
        }

        public static void AssignAllTypeConverters(IDictionary<Type, Converter<HoconValue, object>> converters)
        {
            cacheLock.EnterWriteLock();
            foreach (var kvp in converters) deserializeFuncs.Add(kvp.Key, kvp.Value);
            cacheLock.ExitWriteLock();
        }

        public static void UnassignTypeConverter(Type T)
        {
            cacheLock.EnterWriteLock();
            deserializeFuncs.Remove(T);
            cacheLock.ExitWriteLock();
        }

        public static void UnassignTypeConvertesr(ICollection<Type> Ts)
        {
            cacheLock.EnterWriteLock();
            foreach (var type in Ts) deserializeFuncs.Remove(type);
            cacheLock.ExitWriteLock();
        }

        /*
        Note:

        大概这么写吧

        1. 在第一次遇到这个类型的时候读取所有 attribute 数据
        2. 读取类型 (Type, HoconConfigAttribute) 和它所有带 attribute 的 field (FieldInfo, HoconNodeAttribute) 和 property (PropertyInfo, HoconNodeAttribute) 的信息，还有这些 field 和 property 所对应的键名
        3. 对于每一个键名，找出它所需要用的反序列化函数（attribute 里面定义的最优先，其次如果全局注册过了用全局的，最后还没有的话判断类型有没有实现 IHoconDeserializable 接口，都没有的话报错）
        4. 把以上所有信息保存到一个 cache，向另一个 cache 保存下面的反序列化函数
        5. 每次碰到需要反序列化这个类型的场合，从 cache 找到这个类型的信息，对每个键执行反序列化函数，并把结果赋值给对象。
         */

        private static Converter<HoconValue, object> GetTrivialDeserializeFunc(Type T, bool checkType = false)
            => (HoconValue val) => DeserializeTrivial(val.GetObject(), T, checkType: checkType);

        public static T Deserialize<T>(HoconValue v, bool checkType = false)
        {
            var tType = typeof(T);
            return (T)deserializeFuncs[tType](v);
        }

        public static object Deserialize(HoconValue v, Type T)
        {
            return deserializeFuncs[T](v);
        }
        public static object Deserialize(HoconValue v, out Type T)
        {
            var obj = v.GetObject();
            var typeIdentifier = obj.GetField("$type").GetString();
            var cfg = cache[typeIdentifier];
            T = cfg.type;
            return DeserializeTrivial(obj, cfg.type, cfg, false);
        }

        public static object DeserializeTrivial(
            HoconObject obj, Type T,
            HoconConfigCache cfg = null, bool checkType = false)
        {
            cfg = cfg ?? _CacheType(T);

            if (checkType)
                if (obj.GetField("$type").GetString() != cfg.conf.typeIdentifier)
                    throw new UnableToDeserializeHoconException();

            var result = T.GetConstructor(new Type[0]).Invoke(new object[0]);

            foreach (var kvp in obj)
            {
                if (!cfg.nodes.ContainsKey(kvp.Key)) continue;
                var attr = cfg.nodes[kvp.Key];
                var value = attr.converter(kvp.Value.Value);
                if (attr.isProperty)
                {
                    attr.propertyInfo.SetValue(result, value);
                }
                else
                {
                    attr.fieldInfo.SetValue(result, value);
                }
            }
            return result;
        }

        // Code shamelessly copied from this StackOverflow question:
        // https://stackoverflow.com/questions/951536/
        private static bool isArray(Type T, out int rank, out Type innerType)
        {
            if (T.IsArray)
            {
                rank = T.GetArrayRank();
                innerType = T.GetGenericArguments()[0];
                return true;
            }
            else
            {
                // Who cares
                rank = 0;
                innerType = null;
                return false;
            }
        }

        private static bool isICollection(Type T, out Type innerType)
        {
            var listTest =
                new Predicate<Type>(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));

            if (listTest(T))
            {
                innerType = T.GenericTypeArguments[0];
                return true;
            }

            foreach (Type i in T.GetInterfaces())
            {
                if (listTest(i))
                {
                    innerType = i.GenericTypeArguments[0];
                    return true;
                }
            }
            // no one cares about innertype
            innerType = null;
            return false;
        }

        private static bool isIDictionary(Type T, out Type keyType, out Type valueType)
        {
            var dictionaryTest =
                new Predicate<Type>(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));

            if (dictionaryTest(T))
            {
                keyType = T.GenericTypeArguments[0];
                valueType = T.GenericTypeArguments[1];
                return true;
            }

            foreach (Type i in T.GetInterfaces())
            {
                if (dictionaryTest(i))
                {
                    keyType = i.GenericTypeArguments[0];
                    valueType = i.GenericTypeArguments[1];
                    return true;
                }
            }
            // no one cares about innertype
            keyType = null;
            valueType = null;
            return false;
        }

        public static IList<T> ParseList<T>(HoconValue e, Converter<HoconValue, T> conv)
        {
            return e.GetArray().ConvertAll(conv);
        }

        public static IDictionary<string, TVal> ParseDictionary<TVal>(HoconValue e, Converter<HoconValue, TVal> valConv)
        {
            var dict = new Dictionary<string, TVal>();
            foreach (var kvp in e.GetObject())
            {
                dict.Add(kvp.Key, valConv(kvp.Value.Value));
            }
            return dict;
        }

        public static IDictionary<TKey, TVal> ParseDictionary<TKey, TVal>(HoconValue e, Converter<string, TKey> keyConv, Converter<HoconValue, TVal> valConv)
        {
            var dict = new Dictionary<TKey, TVal>();
            foreach (var kvp in e.GetObject())
            {
                dict.Add(keyConv(kvp.Key), valConv(kvp.Value.Value));
            }
            return dict;
        }

    }

    public class HoconConfigCache
    {
        public Type type;
        public bool isDeserializerTrivial;
        public HoconConfigAttribute conf;
        public IDictionary<string, HoconNodeAttribute> nodes;
    }

    public class UnableToDeserializeHoconException : Exception
    {
        string msg;
        public override string Message { get => msg; }
        public UnableToDeserializeHoconException(string msg = null)
        {
            this.msg = msg;
        }
    }

    class RequireTypeToBeResolvedLaterException : Exception
    {
        Type T;
        public override string Message { get => $"The converter function of {T} is requested to be resolved later"; }
        public RequireTypeToBeResolvedLaterException(Type T) { this.T = T; }
    }

    [System.AttributeUsage(
        System.AttributeTargets.Class | System.AttributeTargets.Struct,
        Inherited = true)]
    /// <summary>
    /// Indicating one class or struct can be deserialized from a HOCON config file or node
    /// </summary>
    public class HoconConfigAttribute : Attribute
    {
        /// <summary>
        /// The <c>$type</c> key in the 
        /// </summary>
        public string typeIdentifier;

        /// <summary>
        /// Initialize with a type identifier. 
        /// This makes deserialization with general documents easier.
        /// </summary>
        /// <param name="typeIdentifier"></param>
        public HoconConfigAttribute(string typeIdentifier)
        {
            this.typeIdentifier = typeIdentifier;
        }

        /// <summary>
        /// Initialize without type identifier.
        /// </summary>
        public HoconConfigAttribute()
        {

        }
    }

    [System.AttributeUsage(
        System.AttributeTargets.Field | System.AttributeTargets.Property,
        Inherited = true)]
    public class HoconNodeAttribute : Attribute
    {
        public string key;
        public Converter<HoconValue, object> converter;

        public bool isProperty;
        public bool isField { get => !isProperty; set => isProperty = !value; }
        public bool isArray;
        public bool isList;
        public bool isDictionary;
        public Type elementType;
        public Type keyType;
        public FieldInfo fieldInfo;
        public PropertyInfo propertyInfo;

        public Type Type { get => isProperty ? propertyInfo.DeclaringType : fieldInfo.DeclaringType; }

        public HoconNodeAttribute(string key, Converter<HoconValue, object> converter = null)
        {
            this.key = key;
            this.converter = converter;
        }
        public HoconNodeAttribute() { }
    }

    public interface IHoconDeserializable
    {
        void ReadFromHocon(HoconValue e);
    }


    /// <summary>
    /// An exception indicating that the object currently reading does not meet the expectation 
    /// of what should be
    /// </summary>
    [System.Serializable]
    public class DeformedObjectException : System.Exception
    {
        public DeformedObjectException() { }
        public DeformedObjectException(string message) : base(message) { }
        public DeformedObjectException(string message, System.Exception inner) : base(message, inner) { }
        protected DeformedObjectException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
