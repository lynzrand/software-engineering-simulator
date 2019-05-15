using System;
using System.Collections.Generic;
using Ceras;

namespace Sesim.Models
{
    public partial class Company
    {
        // 7200 ticks / day, 300 ticks / hour, should be enough to play with
        public const int ticksPerDay = 7200;
        public const int ticksPerHour = ticksPerDay / 24;

        public bool isInitialized = false;

        public string name;

        /// <summary>
        /// In-game time measured in ticks. One hour in game time equals 300 ticks
        /// </summary>
        /// <value></value>
        public double ut;

        public decimal fund;

        public float reputation;

        // TODO: add "cache" stuff for quick accessing of tasks and/or employees via identifier
        public List<Contract> contracts;
        public List<Employee> employees;

        public List<WorkPeriod> workTimes;

        // Reserved for mods
        public Dictionary<string, dynamic> extraData;

        /// <summary>
        /// Initialize before first play
        /// </summary>
        public void Init(string name = "")
        {
            this.name = name;
            this.ut = 0;
            this.fund = decimal.Zero;
            this.reputation = 0f;
            this.contracts = new List<Contract>();
            this.employees = new List<Employee>();
            this.workTimes = new List<WorkPeriod>
            {
                new WorkPeriod(2700, 3450),
                new WorkPeriod(3750, 5100)
            };
        }

        [Exclude]
        public bool IsInWorkTime { get => workTimes.Exists(period => period.isInPeriod(ut)); }

        /// <summary>
        /// Increase time and recalculate params
        /// </summary>
        /// <param name="deltaT">The amount of time to be increased</param>
        public void FixedUpdate(double deltaT)
        {
            ut += deltaT;
            // cache
            var isInWorkTime = this.IsInWorkTime;
            // Update employees
            foreach (var employee in employees)
            {
                employee.UpdateWorkStatus(ut, isInWorkTime);
            }
            // Update contracts
            foreach (var contract in contracts)
            {
                contract.UpdateProgress(ut, deltaT);
                contract.AutoCheckStatus(ut);
            }

            // TODO: update showed data
        }

        public void AddEmployee(Employee x)
        {
            employees.Add(x);
        }

        public void RemoveEmployee(Ulid id)
        {
            employees.RemoveAll(e => e.id == id);
        }

        public void AddContract(Contract x)
        {
            contracts.Add(x);
        }

        public void RemoveContract(Ulid id)
        {
            contracts.RemoveAll(c => c.id == id);
        }
    }

    public struct WorkPeriod
    {
        public WorkPeriod(double start, double end) { this.start = start; this.end = end; }

        double start;
        double end;

        public double Start { get => start; set { if (value < Company.ticksPerDay) start = value; } }
        public double End { get => end; set { if (value < Company.ticksPerDay) end = value; } }

        public bool isInPeriod(double ut)
              => (ut % Company.ticksPerDay) >= start && (ut % Company.ticksPerDay) < end;
    }

}
