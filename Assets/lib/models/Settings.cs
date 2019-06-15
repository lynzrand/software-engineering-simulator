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

        bool firstNamesInitialized = false;
        public List<String> employeeFirstNames = new List<string>(){
            "Employee"
        };

        bool lastNamesInitialized = false;
        public List<String> employeeLastNames = new List<string>(){
            "A", "B", "C", "D"
        };

        bool contractorNamesInitialized = false;
        public List<String> contractorNames = new List<string>(){
            "AkkaSoft", "BraunShell", "Catty", "Dynamo"
        };

        public List<ContractFactory> contractFactories = new List<ContractFactory>() {
            new ContractFactory().SetDebugDefault()
        };

        public void ApplyHocon(Hocon.HoconObject obj)
        {
            if (obj.TryGetField("employee-first-names", out var firstNames))
            {
                if (!firstNamesInitialized)
                {
                    employeeFirstNames.Clear();
                    firstNamesInitialized = true;
                }

                var firstNamesArr = firstNames.Value.GetStringList();
                employeeFirstNames.AddRange(firstNamesArr);
            }

            if (obj.TryGetField("employee-last-names", out var lastNames))
            {
                if (!lastNamesInitialized)
                {
                    employeeLastNames.Clear();
                    lastNamesInitialized = true;
                }

                var lastNamesArr = lastNames.Value.GetStringList();
                employeeLastNames.AddRange(lastNamesArr);
            }

            if (obj.TryGetField("company-names", out var companyNames))
            {
                if (!contractorNamesInitialized)
                {
                    contractorNames.Clear();
                    contractorNamesInitialized = true;
                }

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
