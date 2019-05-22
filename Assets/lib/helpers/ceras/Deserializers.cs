using System;
using Ceras;
using Ceras.Formatters;

namespace Sesim.Helpers.Ceras
{
    public class UlidFormatter : IFormatter<Ulid>
    {
        static UlidFormatter()
        {
            CerasSerializer.AddFormatterConstructedType(typeof(Ulid));
        }

        public void Deserialize(byte[] buffer, ref int offset, ref Ulid value)
        {
            var byteSpan = buffer.AsSpan(offset, 16);
            value = new Ulid(byteSpan);
            offset += 16;
        }

        public void Serialize(ref byte[] buffer, ref int offset, Ulid value)
        {
            // Ensures that we have enough space to cram in an Ulid
            SerializerBinary.EnsureCapacity(ref buffer, offset, 16);
            var byteSpan = buffer.AsSpan(offset, 16);
            value.TryWriteBytes(byteSpan);
            // Move 128 bits further
            offset += 16;
        }
    }
}
