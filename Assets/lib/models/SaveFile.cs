using System;
using ProtoBuf;

namespace Sesim.Models
{
    [ProtoContract]
    public class SaveFile
    {
        [ProtoMember(16)]
        public long version = 0;

        [ProtoMember(20)]
        public Company company;


    }
}