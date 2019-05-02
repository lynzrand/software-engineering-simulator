using System;
using Hocon;
using MessagePack;
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
        AnimationCurve abundanceCurve;

        public ContractFactory()
        {

        }

        public void DeserializeFromHocon(IHoconElement rootNode)
        {
            if (!(rootNode is HoconObject)) throw new DeformedObjectException();
            throw new NotImplementedException();
        }
    }

    public partial class Contract
    {
        public Ulid id;
        public string name;
        public string description;
        public Ulid[] members;
        public Ulid taskId;
        [IgnoreMember]
        public CompanyTask task;
        public float difficulty;

    }

    [MessagePackObject(keyAsPropertyName: true)]
    public partial class CompanyTask
    {
        public Ulid id;

        public string name;

        public float totalWorkload;


        public float testCoverage;

        public bool testPassed;
    }

    public partial interface ICompleteCondition
    {
        // Predicate<CompanyTask> ConditionTester { get; }
        bool TestCondition(CompanyTask task);
    }
}