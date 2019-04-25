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

        private static Mutex cacheMutex = new Mutex();

        private static Dictionary<Type, Converter<HoconValue, object>> deserializeFuncs =
            new Dictionary<Type, Converter<HoconValue, object>>();

        public static void ClearCache()
        {
            cache.Clear();
        }

        public static void CacheType(Type T)
        {
            _CacheType(T);
        }

        internal static HoconConfigCache _CacheType(Type T)
        {
            // Tries to get the specific attribute
            var attr = T.GetCustomAttribute(typeof(HoconConfigAttribute)) as HoconConfigAttribute;
            if (attr == null) throw new UnableToDeserializeHoconException();

            if (cache.ContainsKey(attr.typeIdentifier)) return cache[attr.typeIdentifier];

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
                    if (a.converter == null)
                    {
                        if (typeof(IHoconDeserializable).IsAssignableFrom(type))
                        {
                            var methodInfo =
                                type.GetRuntimeMethod(
                                    "ReadFromHocon",
                                    new Type[] { typeof(HoconValue) });

                            a.converter =
                                (HoconValue v) =>
                                    methodInfo.Invoke(
                                        type.TypeInitializer.Invoke(new object[0]),
                                        new object[] { v });
                        }
                        else
                        {
                            try
                            {
                                _CacheType(type);
                                a.converter = GetDeserializeFunc(type);
                            }
                            catch (Exception e)
                            {
                                throw new UnableToDeserializeHoconException();
                            }
                        }
                    }
                    nodes.Add(a.key ?? prop.Name, a);
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
                    if (a.converter == null)
                    {
                        if (typeof(IHoconDeserializable).IsAssignableFrom(type))
                        {
                            var methodInfo =
                                type.GetRuntimeMethod(
                                    "ReadFromHocon",
                                    new Type[] { typeof(HoconValue) });

                            a.converter =
                                (HoconValue v) =>
                                    methodInfo.Invoke(
                                        type.TypeInitializer.Invoke(new object[0]),
                                        new object[] { v });
                        }
                        else
                        {
                            try
                            {
                                _CacheType(type);
                                a.converter = GetDeserializeFunc(type);
                            }
                            catch (Exception e)
                            {
                                throw new UnableToDeserializeHoconException();
                            }
                        }
                    }
                    nodes.Add(a.key ?? field.Name, a);
                }
            }
            var configCache = new HoconConfigCache()
            {
                type = T,
                conf = attr,
            };

            // Waits so only one thread can cache the type
            cacheMutex.WaitOne();

            // If other threads have already added this class when we are processing,
            // just return the result.
            if (cache.ContainsKey(attr.typeIdentifier)) return configCache;

            cache.Add(attr.typeIdentifier, configCache);
            deserializeFuncs.Add(T, GetDeserializeFunc(T));

            // Allow other threads to modify
            cacheMutex.ReleaseMutex();

            return configCache;
        }

        // public static dynamic Deserialize(IHoconElement el)
        // {
        //     if (el is HoconLong) return long.Parse((el as HoconLong).Value);
        //     else if (el is HoconDouble) return double.Parse((el as HoconDouble).Value);
        //     else if (el is HoconBool) return bool.Parse((el as HoconBool).Value);
        //     else if (el is HoconQuotedString) return (el as HoconQuotedString).Value;
        //     else if (el is HoconUnquotedString) return (el as HoconUnquotedString).Value;
        //     else if (el is HoconTripleQuotedString) return (el as HoconTripleQuotedString).Value;
        //     else if (el is HoconHex)
        //         return long.Parse((el as HoconHex).Value, System.Globalization.NumberStyles.HexNumber);
        //     else if (el is HoconArray)
        //     {
        //         var arr = el as HoconArray;
        //         var result = new List<dynamic>();

        //         foreach (var i in arr)
        //         {
        //             result.Add(Deserialize(i));
        //         }
        //         return result;
        //     }
        //     else if (el is HoconObject)
        //     {
        //         var obj = el as HoconObject;
        //         var typeIndicator = obj["$type"].Value.GetString();
        //         if (!cache.ContainsKey(typeIndicator))
        //             throw new HoconParserException("Unable to determine the object's type");

        //         var typeCache = cache[typeIndicator];
        //         var type = typeCache.type;
        //         var instance = type.TypeInitializer.Invoke(new object[0]);

        //         foreach (var kvp in obj)
        //         {
        //             if (typeCache.fields.ContainsKey(kvp.Key))
        //             {
        //                 var field = typeCache.fields[kvp.Key];
        //                 var attr = field.Item1;
        //                 var fieldInfo = field.Item2;
        //                 var converter = attr.converter ?? deserializeFuncs[fieldInfo.FieldType];
        //                 if (converter == null) throw new UnableToDeserializeHoconException();
        //                 var convetedValue = converter(kvp.Value.Value);
        //             }
        //         }
        //     }
        //     else throw new UnableToDeserializeHoconException();
        // }

        private static Converter<HoconValue, object> GetDeserializeFunc(Type T, bool checkType = false)
            => (HoconValue val) => Deserialize(val, T, checkType);

        public static T Deserialize<T>(HoconValue v, bool checkType = false)
        {
            var tType = typeof(T);
            return (T)Deserialize(v, tType, checkType);
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

        public static object Deserialize(HoconValue el, Type T, bool checkType = false)
        {
            // TODO: Write func according to note
            var cfg = _CacheType(T);

            throw new NotImplementedException();
        }
    }

    struct HoconConfigCache
    {
        public Type type;
        public HoconConfigAttribute conf;
        public IDictionary<string, HoconNodeAttribute> nodes;
    }

    public class UnableToDeserializeHoconException : Exception
    {
        public UnableToDeserializeHoconException() { }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct,
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

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property,
     Inherited = true)]
    public class HoconNodeAttribute : Attribute
    {
        public string key;
        public Converter<HoconValue, object> converter;

        public bool isProperty;
        public bool isField { get => !isProperty; set => isProperty = !value; }
        public FieldInfo fieldInfo;
        public PropertyInfo propertyInfo;
        public HoconNodeAttribute(string key, Converter<HoconValue, object> converter = null)
        {
            this.key = key;
            this.converter = converter;
        }
        public HoconNodeAttribute() { }
    }

    public interface IHoconDeserializer<T>
    {
        Type ExpectedType { get; }
        T ParseHocon(HoconValue e);
    }

    public interface IHoconDeserializable
    {
        void ReadFromHocon(HoconValue e);
    }

}