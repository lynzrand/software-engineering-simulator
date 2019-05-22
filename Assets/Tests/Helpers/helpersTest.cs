using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Helpers.UI;

namespace Tests
{
    public class HelpersTest
    {
        [Test]
        public void TestProgressBarGenerator()
        {
            var bar1 = ConsoleHelper.GenerateProgressBar(10, 0.5f);
            var bar2 = ConsoleHelper.GenerateProgressBar(10, 0.01f);
            var bar3 = ConsoleHelper.GenerateProgressBar(10, 0f);
            var bar4 = ConsoleHelper.GenerateProgressBar(10, 1f);
            var bar5 = ConsoleHelper.GenerateProgressBar(10, 0.95f);
            Assert.That(bar1, Is.EqualTo("[===>    ]"));
            Assert.That(bar2, Is.EqualTo("[>       ]"));
            Assert.That(bar3, Is.EqualTo("[        ]"));
            Assert.That(bar4, Is.EqualTo("[========]"));
            Assert.That(bar5, Is.EqualTo("[=======>]"));
            var bar6 = ConsoleHelper.GenerateProgressBar(3, 0f);
            Assert.That(bar6, Is.EqualTo("[ ]"));
            var bar7 = ConsoleHelper.GenerateProgressBar(
                12, 0.6f, '(', ')', '|', '-', '|'
            );
            Assert.That(bar7, Is.EqualTo("(||||||----)"));
        }
    }
}
