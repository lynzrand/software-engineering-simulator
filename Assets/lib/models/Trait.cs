using System;
using MessagePack;

namespace Sesim.Models
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class Trait
    {
        public string name;
    }
}