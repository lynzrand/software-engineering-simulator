using System;
using System.Collections.Generic;
using Ceras;
using Hocon;
using Sesim.Helpers.Config;
using UnityEngine;

namespace Sesim.Models
{
    #region Contract Factory
    public class ContractFactory : IConfDeserializable
    {
        public string name;
        public string category;
        public string title;
        public string description;
        public AnimationCurve abundanceCurve;

        public ContractFactory()
        {

        }

        public void DeserializeFromHocon(IHoconElement rootNode)
        {
            if (!(rootNode is HoconObject)) throw new DeformedObjectException();
            throw new NotImplementedException();
        }
    }

    #endregion


    #region Contract

    public enum ContractStatus
    {
        Open = 0,
        Standby = 4,
        Working = 5,
        Maintaining = 6,
        Finished = 7,
        Aborted = 16,
        Other = 32
    }

    /// <summary>
    /// Represents the reward (or punishment, if applicable) of a contract
    /// </summary>
    public class ContractReward
    {
        public decimal fund;
        public float reputation;
    }

    /// <summary>
    /// <para> Represents a contract from a contractor. </para>
    /// <para> Inspired by contracts in the game "Kerbal Space Program". </para>
    /// </summary>
    public class Contract
    {
        #region Contract - Basic Stats
        /// <summary>
        /// The unique ID associated with this contract
        /// </summary>
        public Ulid id;

        /// <summary>
        /// The name or short summary of this contract.
        /// A sample would be "Build a store app for xxx company"
        /// </summary>
        public string name;

        /// <summary>
        /// The status of this contract.
        /// </summary>
        public ContractStatus status;

        /// <summary>
        /// The description of this contract.
        /// </summary>
        public string description;

        /// <summary>
        /// The name of the contractor of this contract. 
        /// Should be "xxx company".
        /// </summary>
        public string contractor;

        /// <summary>
        /// The difficulty of this contract
        /// </summary>
        public float difficulty;

        /// <summary>
        /// True if maintenance is avaliable for this contract; else once finished it's over
        /// </summary>
        public bool hasExtendedMaintenancePeriod = false;

        #endregion

        #region Contract - Timings

        /// <summary>
        /// The time that this contract spawns.
        /// </summary>
        public int startTime;

        /// <summary>
        /// The time this contract lives to until it disappear in contract view.
        /// </summary>
        public int liveTime;

        /// <summary>
        /// The duration length of this contract's live period
        /// </summary>
        /// <value></value>
        [Exclude]
        public int LiveDuration
        {
            get => liveTime - startTime;
            set => liveTime = startTime + value;
        }

        /// <summary>
        /// The time this contract allows to be worked on to.
        /// </summary>
        public int timeLimit;

        /// <summary>
        /// The duration length of this contract's live period
        /// </summary>
        /// <value></value>
        [Exclude]
        public int LimitDuration
        {
            get => timeLimit - startTime;
            set => timeLimit = startTime + value;
        }

        /// <summary>
        /// The time this contract's support period extends to.
        /// </summary>
        public int extendedTimeLimit;

        /// <summary>
        /// The duration length of this contract's live period.
        /// </summary>
        /// <value></value>
        [Exclude]
        public int ExtendedLimitDuration
        {
            get => extendedTimeLimit - startTime;
            set => extendedTimeLimit = startTime + value;
        }

        #endregion

        #region Contract - Works and Employees

        /// <summary>
        /// The list of employees involved in this contract.
        /// </summary>
        public List<Employee> members;

        /// <summary>
        /// The technology stack preference of this contract.
        /// Higher number for higher preference and higher bonus in work,
        /// 0 or null (no such key) means not applicable.
        /// </summary>
        public Dictionary<string, float> techStackPreference;

        /// <summary>
        /// The technology stack actually used in this project
        /// </summary>
        public string techStack;

        /// <summary>
        /// The total workload of this contract.
        /// </summary>
        public double totalWorkload;

        /// <summary>
        /// The current work amount of this contract.
        /// </summary>
        public double completedWork;

        [Exclude]
        public float Progress { get => (float)(completedWork / totalWorkload); }

        /// <summary>
        /// The reward the user's company receives upon deposit.
        /// </summary>
        public ContractReward depositReward;

        public void UpdateProgress(int ut, int dTime = 1)
        {
            float delta = 0f;
            foreach (var employee in members)
            {
                var employeeEfficiency = employee.GetEfficiency(techStack, ut);
                var work = employeeEfficiency * dTime;
                delta += work;
            }
            completedWork += delta;
        }

        public void AutoCheckStatus(int ut)
        {
            switch (status)
            {
                case ContractStatus.Working when completeCondition.CompleteTest(ut, this):
                    if (hasExtendedMaintenancePeriod) status = ContractStatus.Maintaining;
                    else status = ContractStatus.Finished;
                    break;
                case ContractStatus.Working when completeCondition.BreakTest(ut, this):
                    status = ContractStatus.Aborted;
                    break;
                case ContractStatus.Maintaining when completeCondition.CompleteMaintenanceTest(ut, this):
                    status = ContractStatus.Finished;
                    break;
            }
        }

        #endregion

        #region Contract - Reward and completion

        /// <summary>
        /// The reward the user's company receives once this project is completed.
        /// </summary>
        public ContractReward completeReward;

        /// <summary>
        /// The reward the user's company receives in maintenance period.
        /// </summary>
        public ContractReward maintenanceMonthlyReward;

        /// <summary>
        /// The punishment the user's company receives if he breaks the contract.
        /// </summary>
        public ContractReward breakContractPunishment;

        /// <summary>
        /// The complete condition of this contract.
        /// </summary>
        public ICompleteCondition completeCondition = new TrivialCompleteCondition();

        #endregion
    }

    public interface ICompleteCondition
    {
        // Predicate<CompanyTask> ConditionTester { get; }
        bool CompleteTest(int ut, Contract task);
        bool CompleteMaintenanceTest(int ut, Contract task);
        bool BreakTest(int ut, Contract task);
    }

    public class TrivialCompleteCondition : ICompleteCondition
    {
        public bool BreakTest(int ut, Contract task)
        {
            return task.timeLimit < ut;
        }

        public bool CompleteMaintenanceTest(int ut, Contract task)
        {
            return task.extendedTimeLimit < ut;
        }

        public bool CompleteTest(int ut, Contract task)
        {
            return task.completedWork >= task.totalWorkload;
        }
    }
    #endregion
}
