using System;
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
            var employee = new Employee
            {
                id = Ulid.NewUlid(),
                name = name,
                baseEfficiency = base_efficiency,
                experience = experience,
                salary = salary,
            };
            return employee;
        }

        // TODO: Read this from file
        private static string[] employeeNames = {
            "A people", "B people", "C people", "D people"
        };

        public String RandomName()
        {
            return employeeNames[new System.Random().Next(employeeNames.Length)];
        }

        public float RandomEfficiency()
        {
            return (float)LogNormal.Sample(0.5, 0.4);
        }
        public float RandomExperience()
        {
            return (float)LogNormal.Sample(1.0, 1.0);
        }

        public decimal RandomSalary(float exp)
        {
            decimal baseSalary = 3000m;
            return baseSalary + new decimal(Math.Round(Mathf.Log(exp + 1, 2) * 1000, 2));
        }

    }
}
