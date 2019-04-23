using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Hocon;

namespace Sesim.Helpers.Config
{
    public static class HoconConfigDeserializer
    {
        private static Dictionary<string, HoconConfigCache> cache =
            new Dictionary<string, HoconConfigCache>();

        private static Mutex cacheMutex = new Mutex();

        private static Dictionary<Type, Func<IHoconElement, object>> deserializeFuncs =
            new Dictionary<Type, Func<IHoconElement, object>>();

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

            var props = new Dictionary<string, PropertyInfo>();
            foreach (var prop in allProps)
            {
                // get prop attribute
                var pa = prop.GetCustomAttributes(typeof(HoconNodeAttribute)) as IEnumerable<HoconNodeAttribute>;

            }

            var fields = new Dictionary<string, FieldInfo>();
            foreach (var field in allFields)
            {
                var fattr = field.GetCustomAttribute(typeof(HoconNodeAttribute)) as IEnumerable<HoconNodeAttribute>;

            }
            var configCache = new HoconConfigCache()
            {
                type = tType,
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

        public static T Deserilalize<T>(IHoconElement el, bool checkType = false) where T : new()
        {
            var conf = _CacheType(typeof(T));
            var newT = new T();

            foreach (var kvp in conf.props)
            {
                var name = kvp.Key;
                var prop = kvp.Value;
                // TODO write code that actually assigns value
                prop.SetValue(newT, 0);
            }

            return newT;
        }
    }

    struct HoconConfigCache
    {
        public Type type;
        public IDictionary<string, PropertyInfo> props;
        public IDictionary<string, FieldInfo> fields;
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
        public delegate object TransformFunc(IHoconElement el);
        public HoconNodeAttribute(string key) { this.key = key; }
        public HoconNodeAttribute() { }
    }
}