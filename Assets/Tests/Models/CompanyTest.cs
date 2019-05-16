using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Models;
using System;

namespace Tests.Models
{
    public class CompanyTest
    {
        Company mockCompany;

        [SetUp]
        public void SetUp()
        {
            mockCompany = new Company();
            mockCompany.Init();
            mockCompany.employees.Add(new Employee()
            {
                id = Ulid.NewUlid(),
                name = "Someone",
                baseEfficiency = 1,
                abilities = new Dictionary<string, float>()
                {
                    ["csharp"] = 1.0f
                },
                isWorking = false
            });
            mockCompany.contracts.Add(new Contract()
            {
                id = Ulid.NewUlid(),
                name = "Blah blah contract",
                status = ContractStatus.Working,
                members = new List<Employee>() { mockCompany.employees[0] },
                timeLimit = 7200 * 16,
                totalWorkload = 30.0,
                completedWork = 0.0,
                techStack = "csharp"
            });
        }


        /// <summary>
        /// This test tests about company updates. As the algorithm is too complicated
        /// to be tested quantitatively, this test will be performed qualitatively.
        /// 
        /// Se other more detailed tests for quantitative results.
        /// </summary>
        [Test]
        public void CompanyUpdateTest()
        {
            // Update by 1 tick
            // 0:00 is not in worktime. Should not have any difference after update.
            mockCompany.ut = 0.0;
            mockCompany.Update(1.0);
            Assert.That(mockCompany.contracts[0].Progress, Is.EqualTo(0.0), "Company should not update when not in work period");
            Assert.That(mockCompany.IsInWorkTime, Is.False, "0:00 is not in default work time");

            // 9:00 is in work time. Should start working
            mockCompany.ut = 9 * 300;
            mockCompany.Update(1.0);
            Assert.That(mockCompany.contracts[0].Progress, Is.GreaterThan(0.0), "Company should update when in work period.");
            Assert.That(mockCompany.IsInWorkTime, Is.True, "9:00 is in work period");
        }

        /// <summary>
        /// This test tests about adding and removing contracts from/to the company
        /// </summary>
        [Test]
        public void AddRemoveContractTest()
        {
            var newContract = new Contract()
            {
                id = Ulid.NewUlid(),
                name = "Blah blah other contract",
                status = ContractStatus.Working,
                members = new List<Employee>() { mockCompany.employees[0] },
                timeLimit = 7200 * 16,
                totalWorkload = 30.0,
                completedWork = 0.0,
                techStack = "csharp"
            };
            var oldContractCount = mockCompany.contracts.Count;
            mockCompany.AddContract(newContract);

            Assert.That(mockCompany.contracts.Count, Is.EqualTo(oldContractCount + 1), "Contract count should increase after adding");
            Assert.That(mockCompany.contracts.Contains(newContract), "The new contract should be added");

            Assert.That(mockCompany.RemoveContract(newContract.id), Is.True, "One contract should be removed by this call");
            Assert.That(!mockCompany.contracts.Contains(newContract), "Contract should be removed");
            Assert.That(mockCompany.RemoveContract(newContract.id), Is.False, "The contract should not be avaliable");
        }

        /// <summary>
        /// This test tests about adding and removing employees from/to the company.
        /// </summary>
        [Test]
        public void AddRemoveEmployeeTest()
        {
            var newEmployee = new Employee()
            {
                id = Ulid.NewUlid(),
                name = "Someone Else",
                baseEfficiency = 1,
                abilities = new Dictionary<string, float>()
                {
                    ["csharp"] = 1.0f
                },
                isWorking = false
            };
            var oldEmployeeCount = mockCompany.employees.Count;
            mockCompany.AddEmployee(newEmployee);

            Assert.That(mockCompany.employees.Count, Is.EqualTo(oldEmployeeCount + 1), "Employee count should increase after adding");
            Assert.That(mockCompany.employees.Contains(newEmployee), "The employee should be added");

            Assert.That(mockCompany.RemoveEmployee(newEmployee.id), Is.True, "The employee should be removed by this call");
            Assert.That(!mockCompany.employees.Contains(newEmployee), "The employee should be removed");
            Assert.That(mockCompany.RemoveEmployee(newEmployee.id), Is.False, "The employee should not be avaliable");
        }
    }
}
