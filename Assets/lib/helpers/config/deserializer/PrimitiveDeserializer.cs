using System;
using System.Collections.Generic;
using Hocon;

namespace Sesim.Helpers.Config.Deserializer
{

    // public static class GenericHoconDeserializer
    // {
    //     public static T ParseHocon<T>(IHoconElement e) where T : new()
    //     {
    //         if (e is HoconObject)
    //         {
    //             var eo = e as HoconObject;
    //             var t = new T();
    //             var thisMethod = typeof(GenericHoconDeserializer).GetMethod("ParseHocon");
    //             foreach (var k in eo.Keys)
    //             {
    //                 var f = typeof(T).GetField(k);
    //                 if (f != null)
    //                 {
    //                     var ftype = f.FieldType;
    //                     var genericMethod = thisMethod.MakeGenericMethod(ftype);
    //                     f.SetValue(t, genericMethod.Invoke(null, new object[] { eo[k] }));
    //                 }
    //             }
    //             return t;
    //         }
    //         else throw new ArgumentException();
    //     }
    // }



    public class ByteDeserializer : IHoconDeserializer<Byte>
    {
        public Type ExpectedType => typeof(Byte);

        public Byte ParseHocon(IHoconElement e)
        {
            if (e is HoconLong) return Byte.Parse(((HoconLong)e).Value);
            else throw new Exception($"Expected integer at {e.Raw}");
        }
    }
    public class ShortDeserializer : IHoconDeserializer<short>
    {
        public Type ExpectedType => typeof(short);

        public short ParseHocon(IHoconElement e)
        {
            if (e is HoconLong) return short.Parse(((HoconLong)e).Value);
            else throw new Exception($"Expected integer at {e.Raw}");
        }
    }
    public class IntDeserializer : IHoconDeserializer<int>
    {
        public Type ExpectedType => typeof(int);

        public int ParseHocon(IHoconElement e)
        {
            if (e is HoconLong) return int.Parse(((HoconLong)e).Value);
            else throw new Exception($"Expected integer at {e.Raw}");
        }
    }

    public class LongDeserializer : IHoconDeserializer<long>
    {
        public Type ExpectedType => typeof(long);

        public long ParseHocon(IHoconElement e)
        {
            if (e is HoconLong) return long.Parse(((HoconLong)e).Value);
            else throw new Exception($"Expected integer at {e.Raw}");
        }
    }

    public class DoubleDeserializer : IHoconDeserializer<double>
    {
        public Type ExpectedType => typeof(double);

        public double ParseHocon(IHoconElement e)
        {
            if (e is HoconDouble) return double.Parse(((HoconLong)e).Value);
            else throw new Exception($"Expected number at {e.Raw}");
        }
    }

    public class FloatDeserializer : IHoconDeserializer<float>
    {
        public Type ExpectedType => typeof(float);

        public float ParseHocon(IHoconElement e)
        {
            if (e is HoconDouble) return float.Parse(((HoconLong)e).Value);
            else throw new Exception($"Expected number at {e.Raw}");
        }
    }

    public class ArrayDeserializer<T> : IHoconDeserializer<IList<T>>
    {
        public ArrayDeserializer(Converter<IHoconElement, T> func)
        {
            this.func = func;
        }
        public Type ExpectedType => throw new NotImplementedException();

        public Converter<IHoconElement, T> func;

        public IList<T> ParseHocon(IHoconElement e)
        {
            if (e is HoconArray)
            {
                var arr = e as HoconArray;
                return arr.ConvertAll(func);
            }
            else throw new Exception($"Expected array at {e.Raw}");
        }
    }

    public class MapDeserializer<TKey, TVal> : IHoconDeserializer<IDictionary<TKey, TVal>>
    {
        public Type ExpectedType => typeof(IDictionary<TKey, TVal>);

        public Converter<string, TKey> keyFunc;
        public Converter<IHoconElement, TVal> valFunc;

        public IDictionary<TKey, TVal> ParseHocon(IHoconElement e)
        {
            if (e is HoconObject)
            {
                var obj = e as HoconObject;
                var map = new Dictionary<TKey, TVal>();
                foreach (var key in obj.Keys)
                {
                    var transKey = keyFunc(key);
                    var transVal = valFunc(obj[key]);
                    map.Add(transKey, transVal);
                }
                return map;
            }
            else throw new Exception($"Expected map at {e.Raw}");
        }
    }
}