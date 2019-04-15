using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Sesim.Library.Models
{
    [ProtoContract]
    public partial class Company
    {
        [ProtoMember(16)]
        public string Name { get; set; }

        [ProtoMember(17)]
        public long Time { get; set; }

        [ProtoMember(18)]
        public long Funds { get; set; }

        [ProtoMember(19)]
        public float Reputation { get; set; }

        /// <summary>
        /// Increase time and recalculate params
        /// </summary>
        /// <param name="step">The amount of time to be increased</param>
        public void Tick(int step = 1)
        {
            Time += step;
        }
    }
}