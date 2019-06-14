using System;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using Sesim.Models;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tests.Models
{
    public class ContractFactoryTest
    {
        ContractFactory factory;

        [SetUp]
        public void SetUpFactory()
        {
            factory = new ContractFactory();
            var factoryDefinition = @"
_type: ContractFactory
name: androidMarketApplication
category: applicationContract
title: ""Make a market app in Android for $contractor""
description: ""The mobile world is growing, and $contractor has decided they should sell things in the Internet. They want someone to build an Android application for them.""
abundance: [
    [0, 40]
    [30, 25]
    [100, 12]
]
duration: [
    [0, 2100]
    [30, 3600]
    [100, 12000]
]
workload: [
    [0, 200]
    [50, 500],
    [100, 1250]
]
requirements: {
    programming-language: {
        _in: [java, kotlin, csharp, dart]
    }
}
reward: {
    deposit: {
        fund: 12000
        reputation: 1
    }
    finish: {
        fund: 100000
        reputation: 15
    }
    abort: {
        fund: -12000
        reputation: -3
    }
}
difficulty-multiplier: {
    method: exponential
    workload: 1.06
    fund: 1.08
    reputation: 1.21
}";
            factory.ReadFromHocon(Hocon.Parser.Parse(factoryDefinition).Value);
        }

        [Test]
        public void ReadFactoryTest()
        {
            var factoryDefinition = @"
_type: ContractFactory
name: androidMarketApplication
category: applicationContract
title: ""Make a market app in Android for $contractor""
description: ""The mobile world is growing, and $contractor has decided they should sell things in the Internet. They want someone to build an Android application for them.""
abundance: [
    [0, 40]
    [30, 25]
    [100, 12]
]
duration: [
    [0, 50400]
    [30, 86400]
    [100, 288000]
]
workload: [
    [0, 200]
    [50, 500],
    [100, 1250]
]
requirements: {
    programming-language: {
        _in: [java, kotlin, csharp, dart]
    }
}
reward: {
    deposit: {
        fund: 12000
        reputation: 1
    }
    finish: {
        fund: 100000
        reputation: 15
    }
    abort: {
        fund: -12000
        reputation: -3
    }
}
difficulty-multiplier: {
    method: exponential
    workload: 1.06
    fund: 1.08
    reputation: 1.21
}";
            var factory = new ContractFactory();
            factory.ReadFromHocon(Hocon.Parser.Parse(factoryDefinition).Value);
            Assert.That(factory.name, Is.EqualTo("androidMarketApplication"));
            Assert.That(factory.abundanceCurve, Is.EqualTo(new AnimationCurve(new Keyframe[]{
                new Keyframe(0,40),
                new Keyframe(30,25),
                new Keyframe(100,12),
            }
            )));
            // The ContractFactory#ReadFromHocon method actually does not need testing. It's extremely simple and if anything goes wrong it's the Hocon package's fault.
        }
        [Test]
        public void ReadFactoryTestFailing()
        {
            var factoryDefinition = @"
_type: ContractFactory
name: androidMarketApplication
category: applicationContract
title: ""Make a market app in Android for $contractor""
description: ""The mobile world is growing, and $contractor has decided they should sell things in the Internet. They want someone to build an Android application for them.""
abundance: [
    [0, 40]
    [30, 25]
    [100, 12]
]
workload: [
    [0, 200]
    [50, 500],
    [100, 1250]
]
requirements: {
    programming-language: {
        _in: [java, kotlin, csharp, dart]
    }
}
reward: {
    deposit: {
        fund: 12000
        reputation: 1
    }
    finish: {
        fund: 100000
        reputation: 15
    }
    abort: {
        fund: -12000
        reputation: -3
    }
}
difficulty-multiplier: {
    method: exponential
    workload: 1.06
    fund: 1.08
    reputation: 1.21
}";
            var factory = new ContractFactory();
            Assert.Throws<KeyNotFoundException>(() =>
            factory.ReadFromHocon(Hocon.Parser.Parse(factoryDefinition).Value)
            );
            // The ContractFactory#ReadFromHocon method actually does not need testing. It's extremely simple and if anything goes wrong it's the Hocon package's fault.
        }

        [Test]
        public void ContractGenerationTest()
        {
            var companyTest = new CompanyTest();
            companyTest.SetUp();
            var mockCompany = companyTest.mockCompany;
            var satisfyingTitle = new Regex("Make a market app in Android for [ABCD] company");
            var satisfyingDesc = new Regex("The mobile world is growing, and [ABCD] company has decided they should sell things in the Internet. They want someone to build an Android application for them.");

            var contract = factory.Generate(mockCompany);

            Assert.That(satisfyingTitle.IsMatch(contract.name));
            Assert.That(satisfyingDesc.IsMatch(contract.description));
            Assert.That(contract.status, Is.EqualTo(ContractStatus.Open));
            Assert.That(contract.LiveDuration, Is.EqualTo(15 * 24 * 300));
            Assert.That(contract.depositReward, Is.EqualTo(new ContractReward()
            {
                fund = 12000,
                reputation = 1
            }));
            // This part in code is a simple assignment, so in theory nothing could go wrong. These lines are here to ensure there really isn't.
        }
    }
}
