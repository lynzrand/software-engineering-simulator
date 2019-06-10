using System;
using System.Collections.Generic;
using Ceras;
using Sesim.Helpers.Gameplay;
namespace Sesim.Models
{
    public partial class Company
    {
        // 7200 ticks / day, 300 ticks / hour, should be enough to play with
        public const int ticksPerDay = 7200;
        public const int ticksPerHour = ticksPerDay / 24;
        public const float maxDeltaTPerStep = 10.0f;

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
        public List<Contract> avaliableContracts;
        public List<Contract> contracts;
        public List<Employee> avaliableEmployees;
        public List<Employee> employees;


        public int avaliableEmployeeLimit = 8;

        public int avaliableContractLimit = 5;

        public List<WorkPeriod> workTimes;

        [Exclude]
        public List<IPickedGenerator<Contract, Company>> contractFactories;

        [Exclude]
        public List<IPickedGenerator<Employee, Company>> employeeGenerators;

        // Reserved for mods
        public Dictionary<string, dynamic> extraData;

        [Exclude]
        public string UtString { get => UtToTimeString(ut); }

        [Exclude]
        public (int days, int hours, double minutes) UtTime { get => UtToTime(ut); }

        /// <summary>
        /// Initialize before first play
        /// </summary>
        public void Init(string name = "")
        {
            this.name = name;
            this.ut = 0;
            this.fund = decimal.Zero;
            this.reputation = 0f;
            InitMissingItems();
        }

        public void InitMissingItems()
        {
            this.contracts = this.contracts ?? new List<Contract>();
            this.avaliableContracts = this.avaliableContracts ?? new List<Contract>();
            this.employees = this.employees ?? new List<Employee>();
            this.avaliableEmployees = this.avaliableEmployees ?? new List<Employee>();
            this.contractFactories = this.contractFactories ?? new List<IPickedGenerator<Contract, Company>>{
                new ContractFactory().SetDebugDefault()
            };
            this.employeeGenerators = this.employeeGenerators ?? new List<IPickedGenerator<Employee, Company>>{
                new EmployeeGenerator()
            };
            this.workTimes = this.workTimes ?? new List<WorkPeriod>
            {
                new WorkPeriod(2700, 3450),
                new WorkPeriod(3750, 5100)
            };
        }

        [Exclude]
        public bool IsInWorkTime { get => workTimes.Exists(period => period.isInPeriod(ut)); }

        /// <summary>
        /// Update company status to that after `deltaT` time. May perform multiple 
        /// iterations if `deltaT` is too large.
        /// </summary>
        /// <param name="deltaT">Delta time</param>
        public void Update(double deltaT)
        {
            // Update company stats
            if (deltaT <= maxDeltaTPerStep)
            {
                RawUpdate(deltaT);
            }
            else
            {
                // If time warp multiplier is too large, we need to split the
                // calculation into multiple iterations to preserve accuracy.
                int iterCount = (int)Math.Ceiling(deltaT / maxDeltaTPerStep);
                for (int i = 0; i < iterCount; i++)
                {
                    RawUpdate(deltaT / iterCount);
                }
            }

            // Generate new contracts and employees
            if (avaliableContracts.Count < avaliableContractLimit)
            {
                GenerateContract(avaliableContractLimit - avaliableContracts.Count);
            }
            // if (avaliableEmployees.Count < avaliableEmployeeLimit)
            // {
            //     GenerateContract(avaliableEmployeeLimit - avaliableEmployees.Count);
            // }
        }

        /// <summary>
        /// Re-calculate the company params after time increases
        /// </summary>
        /// <param name="deltaT">The amount of time to be increased</param>
        public void RawUpdate(double deltaT)
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
                CheckContractStatus(contract);
            }
        }

        public void PruneAvaliableEmployees()
        {
            throw new NotImplementedException();
            // avaliableEmployees.RemoveAll(contract => contract.liveTime < ut);
        }

        public void GenerateEmployee(int num = 1)
        {
            throw new NotImplementedException();
        }

        public void PruneAvaliableContracts()
        {
            avaliableContracts.RemoveAll(contract => contract.liveTime < ut);
        }

        public void GenerateContract(int num = 1)
        {
            if (num < 0) throw new ArgumentException("Contract number should be positive!");
            if (num == 0) return;

            if (contractFactories == null || contractFactories.Count == 0) return;

            var picker = new WeightedRandomPicker<IPickedGenerator<Contract, Company>>();

            foreach (var factory in contractFactories)
            {
                picker.AssignCandidate(factory, factory.GetWeight(this));
            }

            for (int i = 0; i < num; i++)
            {
                var factory = picker.Pick();
                avaliableContracts.Add(factory.Generate(this));
            }
        }

        public void AddEmployee(Employee x)
        {
            avaliableEmployees.Remove(x);
            employees.Add(x);
        }

        public bool RemoveEmployee(Ulid id)
        {
            return employees.RemoveAll(e => e.id == id) > 0;
        }

        public void AddContract(Contract x)
        {
            avaliableContracts.Remove(x);
            contracts.Add(x);
        }

        public bool RemoveContract(Ulid id)
        {
            return contracts.RemoveAll(c => c.id == id) > 0;
        }

        /// <summary>
        /// Checks the status of the contract and switches if applicable. This method
        /// also applies the reward to the company.
        /// </summary>
        /// <param name="c"></param>
        public void CheckContractStatus(Contract c)
        {
            var result = c.CheckStatus(ut);
            if (result.changed)
            {
                if (result.oldValue == ContractStatus.Working && result.newValue == ContractStatus.Maintaining)
                {
                    ApplyReward(c.completeReward);
                }
                else if (result.oldValue == ContractStatus.Working && result.newValue == ContractStatus.Finished)
                {
                    ApplyReward(c.completeReward);
                }
                else if (result.oldValue == ContractStatus.Maintaining && result.newValue == ContractStatus.Maintaining)
                {
                    ApplyReward(c.maintenanceMonthlyReward);
                }
                else if (result.newValue == ContractStatus.Aborted)
                {
                    ApplyReward(c.breakContractPunishment);
                }
            }
        }

        /// <summary>
        /// This method unconditionally applies the reward to the company
        /// </summary>
        /// <param name="reward"></param>
        public void ApplyReward(ContractReward reward)
        {
            this.fund += reward.fund;
            this.reputation += reward.reputation;
        }

        public static string UtToTimeString(double ut)
        {
            var time = UtToTime(ut);
            return String.Format("Day{0:D4} {1:D2}:{2:00.0000}", time.days, time.hours, time.minutes);
        }

        public static (int days, int hours, double minutes) UtToTime(double ut)
        {
            int days = (int)Math.Floor(ut / Company.ticksPerDay);
            int hours = (int)Math.Floor((ut % Company.ticksPerDay) / Company.ticksPerHour);
            double minutes = (ut % Company.ticksPerHour) / (Company.ticksPerHour / 60);
            return (days, hours, minutes);
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
