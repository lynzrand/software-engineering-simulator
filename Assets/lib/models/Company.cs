using System;
using System.Collections.Generic;

namespace Sesim.Models
{
    public partial class Company
    {
        // 7200 ticks / day, 300 ticks / hour, should be enough to play with
        public static int TICKS_PER_DAY = 7200;
        public static int TICKS_PER_HOUR = TICKS_PER_DAY / 24;

        public string name;

        /// <summary>
        /// In-game time measured in ticks. One hour in game time equals 300 ticks
        /// </summary>
        /// <value></value>
        public int time;

        public decimal fund;

        public float reputation;

        // TODO: add "cache" stuff for quick accessing of tasks and/or employees via identifier
        public List<CompanyTask> tasks;
        public List<Employee> employees;



        // Reserved for mods
        public Dictionary<string, dynamic> extraData;

        /// <summary>
        /// Increase time and recalculate params
        /// </summary>
        /// <param name="step">The amount of time to be increased</param>
        public void Tick(int step = 1)
        {
            time += step;
            // TODO: add "real" methods to calculate stuff
        }
    }
}
