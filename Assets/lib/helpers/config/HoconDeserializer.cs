using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Hocon;
using Sesim.Helpers.Config.Deserializer;

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

        public static void CacheType(Type t)
        {
            _CacheType(t);
        }

        static HoconConfigCache _CacheType(Type tType)
        {
            // Tries to get the specific attribute
            var attr = tType.GetCustomAttribute(typeof(HoconConfigAttribute)) as HoconConfigAttribute;
            if (attr == null) throw new UnableToDeserializeHoconException();

            if (cache.ContainsKey(attr.typeIdentifier)) return cache[attr.typeIdentifier];

            // Get all properties and fields to find those with HoconNodeAttribute
            var allProps = tType.GetProperties();
            var allFields = tType.GetType().GetFields();

            var props = new Dictionary<string, (HoconNodeAttribute, PropertyInfo)>();
            foreach (var prop in allProps)
            {
                // get prop attribute
                var pa = prop.GetCustomAttributes(typeof(HoconNodeAttribute)) as IEnumerable<HoconNodeAttribute>;
                foreach (var p in pa)
                {
                    props.Add(p.key ?? prop.Name, (p, prop));
                }
            }

            var fields = new Dictionary<string, (HoconNodeAttribute, FieldInfo)>();
            foreach (var field in allFields)
            {
                var fa = field.GetCustomAttribute(typeof(HoconNodeAttribute)) as IEnumerable<HoconNodeAttribute>;
                foreach (var f in fa)
                {
                    fields.Add(f.key ?? field.Name, (f, field));
                }
            }
            var configCache = new HoconConfigCache()
            {
                type = tType,
                conf = attr,
                props = props,
                fields = fields,
            };

            // Waits so only one thread can cache the type
            cacheMutex.WaitOne();

            // If other threads have already added this class when we are processing,
            // just return the result.
            if (cache.ContainsKey(attr.typeIdentifier)) return configCache;

            cache.Add(attr.typeIdentifier, configCache);

            // Allow other threads to modify
            cacheMutex.ReleaseMutex();

            return configCache;
        }

        public static dynamic Deserialize(IHoconElement el)
        {
            if (el is HoconLong) return long.Parse((el as HoconLong).Value);
            else if (el is HoconDouble) return double.Parse((el as HoconDouble).Value);
            else if (el is HoconBool) return bool.Parse((el as HoconBool).Value);
            else if (el is HoconQuotedString) return (el as HoconQuotedString).Value;
            else if (el is HoconUnquotedString) return (el as HoconUnquotedString).Value;
            else if (el is HoconTripleQuotedString) return (el as HoconTripleQuotedString).Value;
            else if (el is HoconHex)
                return long.Parse((el as HoconHex).Value, System.Globalization.NumberStyles.HexNumber);
            else if (el is HoconArray)
            {
                var arr = el as HoconArray;
                var result = new List<dynamic>();

                foreach (var i in arr)
                {
                    result.Add(Deserialize(i));
                }
                return result;
            }
            else if (el is HoconObject)
            {
                var obj = el as HoconObject;
                var typeIndicator = obj["$type"].Value.GetString();
                if (!cache.ContainsKey(typeIndicator))
                    throw new HoconParserException("Unable to determine the object's type");

                var typeCache = cache[typeIndicator];
                var type = typeCache.type;
                var instance = type.TypeInitializer.Invoke(new object[0]);

                foreach (var kvp in obj)
                {
                    if (typeCache.fields.ContainsKey(kvp.Key))
                    {
                        var field = typeCache.fields[kvp.Key];
                        var attr = field.Item1;
                        var fieldInfo = field.Item2;
                        var converter = attr.converter ?? deserializeFuncs[fieldInfo.FieldType];
                        if (converter == null) throw new UnableToDeserializeHoconException();
                        var convetedValue = converter(kvp.Value.Value);
                    }
                }
            }
            else throw new UnableToDeserializeHoconException();
        }

        public static T Deserilalize<T>(HoconValue el, bool checkType = false) where T : new()
        {
            if (typeof(T) == typeof(string))
            {

            }
            var conf = _CacheType(typeof(T));
            var newT = new T();

            if (checkType) { }


            return newT;
        }
    }

    struct HoconConfigCache
    {
        public Type type;
        public HoconConfigAttribute conf;
        public IDictionary<string, (HoconNodeAttribute, PropertyInfo)> props;
        public IDictionary<string, (HoconNodeAttribute, FieldInfo)> fields;
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
        public Converter<IHoconElement, object> converter;
        public HoconNodeAttribute(string key, Converter<IHoconElement, object> converter = null)
        {
            this.key = key;
            this.converter = converter;
        }
        public HoconNodeAttribute() { }
    }
}