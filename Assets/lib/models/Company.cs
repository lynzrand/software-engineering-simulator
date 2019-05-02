using System;
using System.Collections.Generic;

namespace Sesim.Models
{
    public partial class Company
    {
        // 7200 ticks / day, 300 ticks / hour, should be enough to play with
        public static int TICKS_PER_DAY = 7200;
        public static int TICKS_PER_HOUR = TICKS_PER_DAY / 24;

        public string name { get; set; }

        /// <summary>
        /// In-game time measured in ticks. One hour in game time equals 300 ticks
        /// </summary>
        /// <value></value>
        public int time;

        public decimal fund;

        public float reputation;

        // Reserved for mods
        public Dictionary<string, dynamic> extraData;

        /// <summary>
        /// Increase time and recalculate params
        /// </summary>
        /// <param name="step">The amount of time to be increased</param>
        public void Tick(int step = 1)
        {
            time += step;
        }
    }
}