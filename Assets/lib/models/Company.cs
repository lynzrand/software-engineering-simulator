using System;
using System.Collections.Generic;

namespace Sesim.Library.Models
{
    partial class Company
    {

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