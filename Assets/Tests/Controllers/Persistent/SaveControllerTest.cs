using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Sesim.Game.Controllers.Persistent;
using Ceras.Formatters;
using Ceras;
using Ceras.Helpers;
using Ceras.Resolvers;
using Sesim.Models;

namespace Tests.Controllers
{
    public class SaveControllerTest
    {
        SaveController controller;

        [SetUp]
        public void initSaveController()
        {
            controller = SaveController.__DebugNewInstance();
            controller.saveFile = new SaveFile()
            {
                id = System.Ulid.NewUlid(),
                name = "New Save",
                company = new Company()
                {
                    ut = 114514,
                    fund = 1_300_000m,
                    reputation = 3.4f,
                    contracts = new List<Contract>(){
                        new Contract(){
                            id = System.Ulid.NewUlid(),
                            name = "Mock-up Contract",
                        },
                        new Contract(){
                            id = System.Ulid.NewUlid(),
                            name = "Mock-up Contract 2",
                        },
                    },
                    employees = new List<Employee>(){
                        new Employee(){
                            id = System.Ulid.NewUlid(),
                            name = "Mock-up employee",
                        },
                    }
                }
            };
        }

        [Test]
        public void SerializationTest()
        {
            // Proving that Ceras is consistent with this serializing method
            // and Ulid serializes perfectly
            var result = controller.ceras.Serialize<SaveFile>(controller.saveFile);
            var restored = controller.ceras.Deserialize<SaveFile>(result);
            var reserialized = controller.ceras.Serialize<SaveFile>(restored);
            Assert.That(reserialized, Is.EqualTo(result));
        }
        [Test]
        public void MetadataSerializationTest()
        {
            var meta = controller.saveFile.Metadata;
            var serialized = controller.ceras.Serialize<SaveMetadata>(meta);
            var restored = controller.ceras.Deserialize<SaveMetadata>(serialized);
            Debug.Log(meta.ToString() + "; " + restored.ToString());
            Assert.That(meta, Is.EqualTo(restored));
        }
    }
}
