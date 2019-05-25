using System;
using System.Collections.Generic;
using Hocon;

namespace Sesim.Helpers.Config.Deserializer
{
    public static class HoconPrimitiveDeserializers
    {
        static HoconPrimitiveDeserializers()
        {
            // Assign all these parsing functions
            var dict = new Dictionary<Type, Converter<HoconValue, object>>()
            {
                [typeof(bool)] = ParseBool,
                [typeof(byte)] = ParseByte,
                [typeof(short)] = ParseShort,
                [typeof(int)] = ParseInt,
                [typeof(long)] = ParseLong,
                [typeof(float)] = ParseFloat,
                [typeof(double)] = ParseDouble,
                [typeof(IList<bool>)] = ParseBoolList,
                [typeof(IList<byte>)] = ParseByteList,
                [typeof(IList<short>)] = ParseShortList,
                [typeof(IList<int>)] = ParseIntList,
                [typeof(IList<long>)] = ParseLongList,
                [typeof(IList<float>)] = ParseFloatList,
                [typeof(IList<double>)] = ParseDoubleList,
            };
            HoconConfigDeserializer.AssignAllTypeConverters(dict);
        }

        public static object ParseBool(HoconValue e)
        {
            return e.GetBoolean();
        }

        public static object ParseByte(HoconValue e)
        {
            return e.GetByte();
        }

        public static object ParseShort(HoconValue e)
        {
            return (short)e.GetInt();
        }

        public static object ParseInt(HoconValue e)
        {
            return e.GetInt();
        }

        public static object ParseLong(HoconValue e)
        {
            return e.GetLong();
        }

        public static object ParseDouble(HoconValue e)
        {
            return e.GetDouble();
        }

        public static object ParseFloat(HoconValue e)
        {
            return e.GetFloat();
        }

        public static object ParseBoolList(HoconValue e)
        {
            return e.GetBooleanList();
        }

        public static object ParseByteList(HoconValue e)
        {
            return e.GetByteList();
        }

        public static object ParseShortList(HoconValue e)
        {
            return e.GetArray().ConvertAll((HoconValue val) => (short)val.GetInt());
        }

        public static object ParseIntList(HoconValue e)
        {
            return e.GetIntList();
        }

        public static object ParseLongList(HoconValue e)
        {
            return e.GetLongList();
        }

        public static object ParseFloatList(HoconValue e)
        {
            return e.GetFloatList();
        }

        public static object ParseDoubleList(HoconValue e)
        {
            return e.GetDoubleList();
        }
    }
}