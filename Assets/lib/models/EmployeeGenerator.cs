using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using UnityEngine;

namespace Sesim.Models
{
    public class EmployeeGenerator : IPickedGenerator<Employee, Company>
    {
        public float GetWeight(Company c)
        {
            return 1;
        }
        public Employee Generate(Company c)
        {
            var name = RandomName();
            var experience = RandomExperience();
            var base_efficiency = RandomEfficiency();
            var salary = (decimal)RandomSalary(experience);
            var liveDuration = RandomLiveDuration();
            var employee = new Employee
            {
                id = Ulid.NewUlid(),
                name = name,
                baseEfficiency = base_efficiency,
                experience = experience,
                salary = salary,
                liveTime = liveDuration + c.ut,
                abilities = new Dictionary<string, float>
                {
                    ["java"] = 1.0f
                }
            };
            return employee;
        }

        IList<String> employeeFirstNames { get => GlobalSettings.Instance.employeeFirstNames; }
        IList<String> employeeLastNames { get => GlobalSettings.Instance.employeeLastNames; }

        System.Random random = new System.Random();
        public String RandomName()
        {
            var firstName = employeeFirstNames[random.Next(employeeFirstNames.Count)];
            var lastName = employeeLastNames[random.Next(employeeLastNames.Count)];
            return $"{firstName} {lastName}";
        }

        public float RandomEfficiency()
        {
            return (float)LogNormal.Sample(random, 0.5, 0.4);
        }
        public float RandomExperience()
        {
            return (float)LogNormal.Sample(random, 1.0, 1.0);
        }

        public decimal RandomSalary(float exp)
        {
            decimal baseSalary = 3000m;
            return baseSalary + new decimal(Math.Round(Mathf.Log(exp + 1, 2) * 1000, 2));
        }

        public double RandomLiveDuration()
        {
            return LogNormal.Sample(random, 10.3028, 0.5);
        }

    }
}
