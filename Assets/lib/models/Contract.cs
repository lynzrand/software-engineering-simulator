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
        public bool hasMaintenancePeriod = false;

        #endregion

        #region Contract - Timings

        /// <summary>
        /// The time that this contract spawns.
        /// </summary>
        public double startTime;

        /// <summary>
        /// The time this contract lives to until it disappear in contract view.
        /// </summary>
        public double liveTime;

        /// <summary>
        /// The time this contract is accepted by the player.
        /// </summary>
        public double depositTime;

        /// <summary>
        /// The duration length of this contract's live period
        /// </summary>
        /// <value></value>
        [Exclude]
        public double LiveDuration
        {
            get => liveTime - startTime;
            set => liveTime = startTime + value;
        }

        /// <summary>
        /// The time this contract allows to be worked on to.
        /// </summary>
        public double timeLimit;

        /// <summary>
        /// The time this contract is completed by the player.
        /// </summary>
        public double completionTime;

        /// <summary>
        /// The duration length of this contract's live period
        /// </summary>
        /// <value></value>
        [Exclude]
        public double LimitDuration
        {
            get => timeLimit - startTime;
            set => timeLimit = startTime + value;
        }

        /// <summary>
        /// The time this contract's support period extends to.
        /// </summary>
        public double extendedTimeLimit;


        /// <summary>
        /// The duration length of this contract's live period.
        /// </summary>
        /// <value></value>
        [Exclude]
        public double ExtendedLimitDuration
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

        public void UpdateProgress(double ut, double deltaT)
        {
            if (status == ContractStatus.Working || status == ContractStatus.Maintaining)
            {
                double delta = 0;
                foreach (var employee in members)
                {
                    var employeeEfficiency = employee.GetEfficiency(techStack, ut);
                    var work = employeeEfficiency * deltaT / Company.ticksPerHour;
                    delta += work;
                }
                completedWork += delta;
            }
        }

        public TransitionResult<ContractStatus> CheckStatus(double ut)
        {
            ContractStatus oldStatus = status;
            bool changed = true;
            switch (status)
            {
                case ContractStatus.Working when completeCondition.CompleteTest(ut, this):
                    if (hasMaintenancePeriod) status = ContractStatus.Maintaining;
                    else status = ContractStatus.Finished;
                    break;
                case ContractStatus.Working when completeCondition.BreakTest(ut, this):
                    status = ContractStatus.Aborted;
                    break;
                case ContractStatus.Maintaining when completeCondition.ShouldReceiveMaintenanceRewardTest(ut, this):
                    // leave changed = true
                    break;
                case ContractStatus.Maintaining when completeCondition.CompleteMaintenanceTest(ut, this):
                    status = ContractStatus.Finished;
                    break;
                default:
                    changed = false; break;
            }
            return new TransitionResult<ContractStatus>(changed, oldStatus, status);
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

    public struct TransitionResult<T>
    {
        public bool changed;
        public T oldValue;
        public T newValue;

        public TransitionResult(bool changed, T oldValue, T newValue)
        {
            this.changed = changed;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public override bool Equals(object obj)
        {
            return obj is TransitionResult<T> result &&
                   changed == result.changed &&
                   EqualityComparer<T>.Default.Equals(oldValue, result.oldValue) &&
                   EqualityComparer<T>.Default.Equals(newValue, result.newValue);
        }

        public override int GetHashCode()
        {
            var hashCode = 2013722904;
            hashCode = hashCode * -1521134295 + changed.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(oldValue);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(newValue);
            return hashCode;
        }
    }

    public interface ICompleteCondition
    {
        // Predicate<CompanyTask> ConditionTester { get; }
        bool CompleteTest(double ut, Contract task);
        bool CompleteMaintenanceTest(double ut, Contract task);
        bool ShouldReceiveMaintenanceRewardTest(double ut, Contract task);
        bool BreakTest(double ut, Contract task);
    }

    public class TrivialCompleteCondition : ICompleteCondition
    {

        /// <summary>
        /// The time period that the user get rewarded for maintaining.
        /// </summary>
        public double maintenanceRewardPeriod = 30 * 7200;

        [Include]
        public double lastCheckTime = -1;

        public bool BreakTest(double ut, Contract task)
        {
            return task.timeLimit < ut;
        }

        public bool CompleteMaintenanceTest(double ut, Contract task)
        {
            return task.extendedTimeLimit < ut;
        }

        public bool CompleteTest(double ut, Contract task)
        {
            return task.completedWork >= task.totalWorkload;
        }

        public bool ShouldReceiveMaintenanceRewardTest(double ut, Contract task)
        {
            bool result = false;
            result = !(Math.Floor((ut - task.completionTime) / maintenanceRewardPeriod) == Math.Floor((lastCheckTime - task.completionTime) / maintenanceRewardPeriod));
            lastCheckTime = ut;
            return result;
        }
    }
    #endregion
}
