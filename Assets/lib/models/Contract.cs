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

        public float GetWeight(Company C){
            if(C.reputation<100)
            {
                return 1;
            }
            else if(C.reputation>=100&&C.reputation<200)
            {
                
            }
            else{

            }
        }

        public Contract GenerateContract(Company c){
            var contract = new Contract{
                id = Ulid.NewUlid();
                status = ContractStatus.Open;
                contractor=RandomContractor();
                name = RandomName(contractor);
                name + = contractor;
                description = RandomDescription(contractor);
                difficulty = GetWeight(c);
                depositReward = new ContractReward();
                startTime = double ut;
                LiveDuration = 5000;
                LimitDuration = 3000;
            }
        }

        public String RandomContractor(){
            Random rd = new Random();
            String Contractor;
            rd.next(1,4);
            switch(rd){
                case 1:
                    Contractor = "A company";
                    break;
                case 2:
                    Contractor = "B company";
                    break;
                case 3:
                    Contractor = "C company";
                    break;
                case 4:
                    Contractor = "D company";
                    break;
            }
            return Contract;
        }

        public String RandomName(){
            Random rd = new Random();
            String Name;
            rd.next(1,4);
            switch(rd){
                case 1:
                    Name = "Build a store app for ";
                    break;
                case 2:
                    Name = "Build a website for ";
                    break;
                case 3:
                    Name = "Build a service app for ";
                    break;
                case 4:
                    Name = "Fix a store app for ";
                    break;
            }
            return Name;
        }

        public String RandomDescription(String contractor){
            Random rd = new Random();
            String Description = contractor;
            rd.next(1,4);
            switch(rd){
                case 1:
                    Description + = " want to build a store app ,They hava found you ,Please choose whether to help them complete the task";
                    break;
                case 2:
                    Description + = " want to build a website ,They hava found you ,Please choose whether to help them complete the task";
                    break;
                case 3:
                    Description + = " want to build a service app ,They hava found you ,Please choose whether to help them complete the task";
                    break;
                case 4:
                    Description + = " has a app problem ,They hava found you ,Please choose whether to help them fix this problem";
                    break;
            }
            return Description；
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
        public double startTime;

        /// <summary>
        /// The time this contract lives to until it disappear in contract view.
        /// </summary>
        public double liveTime;

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

        public void AutoCheckStatus(double ut)
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
        bool CompleteTest(double ut, Contract task);
        bool CompleteMaintenanceTest(double ut, Contract task);
        bool BreakTest(double ut, Contract task);
    }

    public class TrivialCompleteCondition : ICompleteCondition
    {
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
    }
    #endregion
}