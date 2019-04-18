using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Helpers.UI;

namespace Tests
{
    public class helpersTest
    {
        [Test]
        public void TestProgressBarGenerator()
        {
            var bar1 = ConsoleHelper.GenerateProgressBar(10, 0.5f);
            var bar2 = ConsoleHelper.GenerateProgressBar(10, 0.01f);
            var bar3 = ConsoleHelper.GenerateProgressBar(10, 0f);
            var bar4 = ConsoleHelper.GenerateProgressBar(10, 1f);
            var bar5 = ConsoleHelper.GenerateProgressBar(10, 0.95f);
            Assert.AreEqual("[===>    ]", bar1);
            Assert.AreEqual("[>       ]", bar2);
            Assert.AreEqual("[        ]", bar3);
            Assert.AreEqual("[========]", bar4);
            Assert.AreEqual("[=======>]", bar5);
            var bar6 = ConsoleHelper.GenerateProgressBar(3, 0f);
            Assert.AreEqual("[ ]", bar6);
            var bar7 = ConsoleHelper.GenerateProgressBar(
                12, 0.6f, '(', ')', '|', '-', '|'
            );
            Assert.AreEqual("(||||||----)", bar7);
        }
    }
}
