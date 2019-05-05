using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Models;
using System;

namespace Tests
{
    public class EmployeeTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void EmployeeEfficiencyTest()
        {
            Employee e = new Employee
            {
                id = new Ulid(),
                name = "Test Employee",
                baseEfficiency = 1.5f,
                health = 1.0f,
                pressure = 0f,
                abilities = new Dictionary<string, float>
                {
                    ["java"] = 3f,
                    ["csharp"] = 1.5f,
                    ["php"] = 0.1f,
                    ["lua"] = 0f
                }
            };

            // HealthCurve: [0, 0, 0, 0, 0.33, 0.33] => [1, 1, 0, 0, 0.33, 0.33]
            e.SetEfficiencyHealthCurve();
            // Makes pressureCurve constant in lower pressure
            e.SetEfficiencyPressureCurve(1f, 0.9f, 1f);
            e.SetEfficiencyTimeCurve(1f);

            // Test with basic multiplier
            Assert.That(e.GetEfficiency("java", 0), Is.EqualTo(3f).Within(0.1f), "Known type");
            Assert.That(e.GetEfficiency("lua", 0), Is.EqualTo(0f).Within(.01f), "Known type, zero exp");
            Assert.That(e.GetEfficiency("javascript", 0), Is.EqualTo(0f).Within(.01f), "Unknown type");

            // Test through time
            e.SetEfficiencyTimeCurve();
            Assert.That(e.GetEfficiency("java", 0), Is.EqualTo(1.5f).Within(.01f), "T=0 (startEfficiency)");
            Assert.That(e.GetEfficiency("java", 90), Is.EqualTo(3f).Within(.01f), "T=startTime (max efficiency)");
            Assert.That(e.GetEfficiency("java", 600), Is.EqualTo(3f).Within(.01f), "T=maxTime (max efficiency)");
            Assert.That(e.GetEfficiency("java", 1200), Is.EqualTo(1.5f).Within(.01f), "T=(maxTime+declineTime)/2 (1/2 efficiency)");
            Assert.That(e.GetEfficiency("java", 1800), Is.EqualTo(0f).Within(.01f), "T=declineTime (0 efficiency)");

            // Test through health
            e.health = 0.5f;
            Assert.That(e.GetEfficiency("java", 600), Is.EqualTo(1.5f).Within(.01f), "health=0.5, half efficiency");
            e.health = 0f;
            Assert.That(e.GetEfficiency("java", 600), Is.EqualTo(0f).Within(.01f), "health=0 (0 efficiency)");
            // reset
            e.health = 1f;

            // Test through pressure
            e.SetEfficiencyPressureCurve();
            // no pressure
            e.pressure = 0f;
            Assert.That(e.GetEfficiency("java", 600), Is.EqualTo(2.25f).Within(.01f), "pressure=0 (startEfficiency)");
            // max efficiency
            e.pressure = 0.35f;
            Assert.That(e.GetEfficiency("java", 600), Is.EqualTo(4.8f).Within(.01f), "pressure=maxEfficiencyPressure (maxEfficiency)");
            // max pressure
            e.pressure = 1f;
            Assert.That(e.GetEfficiency("java", 600), Is.EqualTo(1.5f).Within(.01f), "pressure=1 (maxPressureEfficiency)");

        }
    }
}
