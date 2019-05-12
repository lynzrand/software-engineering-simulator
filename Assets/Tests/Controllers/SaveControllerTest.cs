using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Game.Controllers;
using Ceras.Formatters;
using Ceras;
using Ceras.Helpers;
using Ceras.Resolvers;
using Sesim.Models;

namespace Sesim.Tests.Controllers
{
    public class SaveControllerTest
    {
        SaveController controller = new SaveController()
        {
            saveData = new Models.SaveFile()
            {
                id = System.Ulid.NewUlid(),
                name = "New Save",
                company = new Models.Company()
                {
                    ut = 114514,
                    fund = 1_300_000m,
                    reputation = 3.4f,
                    contracts = new List<Models.Contract>(){
                        new Models.Contract(){
                            id = System.Ulid.NewUlid(),
                            name = "Mock-up Contract",
                        },
                    },
                    employees = new List<Models.Employee>(){
                        new Models.Employee(){
                            id = System.Ulid.NewUlid(),
                            name = "Mock-up employee",
                        },
                    }
                }
            }
        };

        [SetUp]
        public void initSaveController()
        {

        }

        [Test]
        public void SerializationTest()
        {
            // Proving that Ceras is consistent with this serializing method
            // and Ulid serializes perfectly
            var result = controller.ceras.Serialize<SaveFile>(controller.saveData);
            var restored = controller.ceras.Deserialize<SaveFile>(result);
            var reserialized = controller.ceras.Serialize<SaveFile>(restored);
            Assert.That(reserialized, Is.EqualTo(result));
        }
        [Test]
        public void MetadataSerializationTest()
        {
            var meta = controller.saveData.Metadata;
            var serialized = controller.ceras.Serialize<SaveMetadata>(meta);
            var restored = controller.ceras.Deserialize<SaveMetadata>(serialized);
            Debug.Log(meta.ToString() + "; " + restored.ToString());
            Assert.That(meta, Is.EqualTo(restored));
        }
    }
}
