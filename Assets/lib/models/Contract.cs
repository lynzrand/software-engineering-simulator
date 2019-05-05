using System;
using System.Collections.Generic;
using Hocon;
using Sesim.Helpers.Config;
using Sesim.Models.Exceptions;
using UnityEngine;
using Wiry.Base32;

namespace Sesim.Models
{
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

    public enum ContractStatus
    {
        Unaccepted,
        Accepted,
        Working,
        Finished,
        Aborted,
        Other
    }

    /// <summary>
    /// Represents a contract from a contractor
    /// </summary>
    public partial class Contract
    {
        public Ulid id;

        public string name;

        public ContractStatus status;

        public string description;

        public string contractor;

        public List<CompanyTask> task;

        public float difficulty;
    }

    /// <summary>
    /// Represents a task in Company
    /// </summary>
    public partial class CompanyTask
    {
        public Ulid id;

        public string name;

        public ContractStatus status;

        public List<Employee> members;

        public float totalWorkload;

        public float testCoverage;


        public ICompleteCondition completeCondition;

        public bool checkCompletion() => completeCondition.TestCondition(this);
    }

    public partial interface ICompleteCondition
    {
        // Predicate<CompanyTask> ConditionTester { get; }
        bool TestCondition(CompanyTask task);
    }
}
