using System;
using System.Collections.Generic;
using Hocon;

namespace Sesim.Helpers.Config.Deserializer
{

    public static class GenericHoconDeserializer
    {
        public static T ParseHocon<T>(IHoconElement e) where T : new()
        {
            if (e is HoconObject)
            {
                var eo = e as HoconObject;
                var t = new T();
                var thisMethod = typeof(GenericHoconDeserializer).GetMethod("ParseHocon");
                foreach (var k in eo.Keys)
                {
                    var f = typeof(T).GetField(k);
                    if (f != null)
                    {
                        var ftype = f.FieldType;
                        var genericMethod = thisMethod.MakeGenericMethod(ftype);
                        f.SetValue(t, genericMethod.Invoke(null, new object[] { eo[k] }));
                    }
                }
                return t;
            }
            else throw new ArgumentException();
        }
    }
    public class IntDeserializer : IHoconDeserializer<int>
    {

    }
}