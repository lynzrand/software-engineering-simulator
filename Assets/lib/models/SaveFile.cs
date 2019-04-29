using System;
using MessagePack;

namespace Sesim.Models
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class SaveFile
    {
        public long version = 0;

        public Company company;


    }
}