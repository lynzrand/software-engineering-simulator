using System;
using System.Collections.Generic;
using Hocon;

public static class HoconPrimitiveDeserializers
{
    public static bool ParseBool(HoconValue e)
    {
        return e.GetBoolean();
    }

    public static byte ParseByte(HoconValue e)
    {
        return e.GetByte();
    }

    public static short ParseShort(HoconValue e)
    {
        return (short)e.GetInt();
    }

    public static int ParseInt(HoconValue e)
    {
        return e.GetInt();
    }

    public static long ParseLong(HoconValue e)
    {
        return e.GetLong();
    }

    public static double ParseDouble(HoconValue e)
    {
        return e.GetDouble();
    }

    public static float ParseFloat(HoconValue e)
    {
        return e.GetFloat();
    }

    public static IList<bool> ParseBoolList(HoconValue e)
    {
        return e.GetBooleanList();
    }

    public static IList<byte> ParseByteList(HoconValue e)
    {
        return e.GetByteList();
    }

    public static IList<short> ParseShortList(HoconValue e)
    {
        return e.GetArray().ConvertAll(ParseShort);
    }

    public static IList<int> ParseIntList(HoconValue e)
    {
        return e.GetIntList();
    }

    public static IList<long> ParseLongList(HoconValue e)
    {
        return e.GetLongList();
    }

    public static IList<float> ParseFloatList(HoconValue e)
    {
        return e.GetFloatList();
    }

    public static IList<double> ParseDoubleList(HoconValue e)
    {
        return e.GetDoubleList();
    }

    public static IList<HoconValue> ParseList(HoconValue e)
    {
        return e.GetArray();
    }

    public static IList<T> ParseList<T>(HoconValue e, Converter<HoconValue, T> conv)
    {
        return e.GetArray().ConvertAll(conv);
    }

    public static IDictionary<string, object> ParseDictionary(HoconValue e)
    {
        return e.GetObject().Unwrapped;
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

    public static IDictionary<TKey, TVal> ParseDictionary<TKey, TVal>(HoconValue e,
    Converter<string, TKey> keyConv, Converter<HoconValue, TVal> valConv)
    {
        var dict = new Dictionary<TKey, TVal>();
        foreach (var kvp in e.GetObject())
        {
            dict.Add(keyConv(kvp.Key), valConv(kvp.Value.Value));
        }
        return dict;
    }
}
