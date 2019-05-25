using System;
using UnityEngine;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Hocon;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Sesim.Helpers.Config
{
    #region readers
    public interface IConfDeserializable
    {
        void DeserializeFromHocon(HoconValue rootNode);
    }

    public class ConfigReader
    {
        static ConfigReader instance = null;

        private ConfigReader()
        {

        }

        public static ConfigReader Instance
        {
            get
            {
                if (instance == null) instance = new ConfigReader();
                return instance;
            }
        }

        public string root;

        // public Queue<string> directoriesToScan = new Queue<string>();

        public ConcurrentDictionary<string, Type> typeDefinitions = new ConcurrentDictionary<string, Type>();

        public ConcurrentDictionary<Type, ConcurrentBag<dynamic>> objects = new ConcurrentDictionary<Type, ConcurrentBag<dynamic>>();

        public void AssignType(string typeAnnotation, Type type)
        {
            if (type.GetInterface("IHoconDeserializable") != null)
                typeDefinitions[typeAnnotation] = type;
            else
                throw new InvalidDataException($"{type.FullName} does not implement IHoconDeserializable.");
        }

        bool readingCompleted = false;
        public bool ReadingCompleted { get => readingCompleted; }
        Task readingTask = null;

        async public void BackgroundReadConfigsAsync(string path)
        {
            readingCompleted = false;
            readingTask = TraverseDirectoriesAsync(path);
            await readingTask;
            readingCompleted = true;
        }

        async public Task TraverseDirectoriesAsync(string dir)
        {
            var files = Directory.EnumerateFiles(dir);
            await Task.WhenAll(files.TakeWhile(file => file.EndsWith(".conf")).Select(file => ReadConfigFile(file)));
            var subDirs = Directory.EnumerateDirectories(dir);
            await Task.WhenAll(subDirs.Select(subdir => TraverseDirectoriesAsync(subdir)));
        }

        async Task ReadConfigFile(string path)
        {
            try
            {
                Debug.Log($"Reading config {path}");
                using (var f = File.OpenText(path))
                {
                    var str = await f.ReadToEndAsync();
                    var config = Parser.Parse(str);
                    HoconValue value = config.Value;
                    var deserializeType = typeDefinitions[value.GetObject()["_type"].GetString()];

                    if (deserializeType == null)
                        throw new Exception($"Type not found when deserializing type \"{value.GetObject()["_type"].GetString()}\"");

                    var ctor = deserializeType.GetConstructor(new Type[0]);
                    var deserializeObject = (IHoconDeserializable)ctor.Invoke(new object[0]);
                    deserializeObject.ReadFromHocon(value);
                    var bag = objects.GetOrAdd(deserializeType, new ConcurrentBag<dynamic>());
                    bag.Add(deserializeObject);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error when reading file \"{path}\"\r\n" + e.ToString());
            }
        }
    }
    #endregion

}
