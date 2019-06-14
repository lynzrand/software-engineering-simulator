using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Helpers.UI;
using System;
using Sesim.Helpers.Ceras;


namespace Tests
{
    public class UlidFormatterTest
    {
        [Test]
        public void TestSerializer()
        {
            var id = Ulid.NewUlid();
            var byteArray = new byte[16];
            var startOffset = 0;
            var formatter = new UlidFormatter();

            formatter.Serialize(ref byteArray, ref startOffset, id);

            Assert.That(startOffset, Is.EqualTo(16));
            Assert.That(byteArray, Is.EqualTo(id.ToByteArray()));
        }

        [Test]
        public void TestDeserializer()
        {
            var id = Ulid.NewUlid();
            var byteArray = new byte[16];
            var startOffset = 0;
            var formatter = new UlidFormatter();

            formatter.Serialize(ref byteArray, ref startOffset, id);
            startOffset = 0;

            Ulid otherId = new Ulid();

            formatter.Deserialize(byteArray, ref startOffset, ref otherId);

            Assert.That(otherId, Is.EqualTo(id));
        }
    }
}
