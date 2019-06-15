using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;

namespace Sesim.Models
{
    public class GlobalSettings
    {
        private static GlobalSettings instance;
        public static GlobalSettings Instance
        {
            get
            {
                return instance ?? (instance = new GlobalSettings());
            }
        }

        public List<String> employeeFirstNames = new List<string>();

        public List<String> employeeLastNames = new List<string>();

        public List<String> contractorNames = new List<string>();

        public List<ContractFactory> contractFactories = new List<ContractFactory>() {
            new ContractFactory().SetDebugDefault()
        };

        public void ApplyHocon(Hocon.HoconObject obj)
        {
            if (obj.TryGetField("employee-first-names", out var firstNames))
            {
                var firstNamesArr = firstNames.Value.GetStringList();
                employeeFirstNames.AddRange(firstNamesArr);
            }

            if (obj.TryGetField("employee-last-names", out var lastNames))
            {
                var lastNamesArr = lastNames.Value.GetStringList();
                employeeLastNames.AddRange(lastNamesArr);
            }

            if (obj.TryGetField("company-names", out var companyNames))
            {
                var companyNamesArr = companyNames.Value.GetStringList();
                contractorNames.AddRange(companyNamesArr);
            }
        }

        public void SetContractFactories()
        {
            if (!Helpers.Config.ConfigReader.Instance.objects.TryGetValue(typeof(ContractFactory), out var rawFactoryDefinitions)) return;

            var factoryDefinitions = rawFactoryDefinitions.Cast<ContractFactory>();
            contractFactories.Clear();
            contractFactories.AddRange(factoryDefinitions);
        }

    }

    public class DifficultySettings
    {
        // public bool enable
    }
}
