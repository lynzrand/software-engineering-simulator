using System;
using System.Collections.Generic;
using Ceras;
using Hocon;
using Sesim.Helpers.Config;
using UnityEngine;

namespace Sesim.Models
{
    public class ContractFactory : IConfDeserializable, IPickedGenerator<Contract, Company>
    {
        public string name;
        public string category;
        public string title;
        public string description;
        public AnimationCurve abundanceCurve;

        public ContractFactory()
        {

        }

        public float GetWeight(Company C)
        {
            if (C.reputation < 100)
            {
                return 1;
            }
            else if (C.reputation >= 100 && C.reputation < 200)
            {

            }
            else
            {

            }
            return 0;
        }

        public Contract Generate(Company c)
        {
            var contractor = RandomContractor();
            var nameDescriptionPair = RandomNameDescription(contractor);
            var contract = new Contract
            {
                id = Ulid.NewUlid(),
                status = ContractStatus.Open,
                contractor = contractor,
                name = nameDescriptionPair.name,
                description = nameDescriptionPair.desc,
                difficulty = GetWeight(c),
                depositReward = new ContractReward(),
                startTime = c.ut,
                LiveDuration = 5000,
                LimitDuration = 3000,
            };
            return contract;
        }

        // TODO: read the followings from file
        private static string[] contractorNames = {
            "A company", "B company", "C company", "D company"
        };

        public String RandomContractor()
        {
            return contractorNames[new System.Random().Next(contractorNames.Length)];
        }


        private static string[] contractTitles = {
            "Build a store app for $contractor",
            "Build a website for $contractor",
        };
        private static string[] contractDescriptions = {
             "$contractor want to build a store app ,They have found you ,Please choose whether to help them complete the task",
             "$contractor want to build a website ,They have found you ,Please choose whether to help them complete the task"
        };


        public (string name, string desc) RandomNameDescription(string contractorName)
        {
            var selected = new System.Random().Next(contractTitles.Length);
            return (
                contractTitles[selected].Replace("$contractor", contractorName),
                contractDescriptions[selected].Replace("$contractor", contractorName)
            );
        }

        public void DeserializeFromHocon(IHoconElement rootNode)
        {
            if (!(rootNode is HoconObject)) throw new DeformedObjectException();
            throw new NotImplementedException();
        }
    }
}
