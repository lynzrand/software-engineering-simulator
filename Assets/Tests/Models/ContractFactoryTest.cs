using System;
using NUnit;
using NUnit.Framework;
using Sesim.Models;
using UnityEngine;

namespace Tests.Models
{
    public class ContractFactoryTest
    {
        ContractFactory factory;

        [SetUp]
        public void SetUpFactory()
        {
            factory = new ContractFactory();
        }

        [Test]
        public void ReadFactory()
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
            var factory = new ContractFactory();
            factory.ReadFromHocon(Hocon.Parser.Parse(factoryDefinition).Value);
            Assert.That(factory.title, Is.EqualTo("androidMarketApplication"));
            Assert.That(factory.abundanceCurve, Is.EqualTo(new AnimationCurve(new Keyframe[]{
                new Keyframe(0,40),
                new Keyframe(30,25),
                new Keyframe(100,12),
            }
            )));
            this.factory = factory;
            // The ContractFactory#ReadFromHocon method actually does not need testing. It's extremely simple and if anything goes wrong it's the Hocon package's fault.
        }


    }
}
