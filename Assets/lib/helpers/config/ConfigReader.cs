using System;
using UnityEngine;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Hocon;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Text;

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
            LogAllConfigTypes();
            readingCompleted = false;
            readingTask = TraverseDirectoriesAsync(path);
            await readingTask;
            readingCompleted = true;
        }

        void LogAllConfigTypes()
        {
            var sb = new StringBuilder("Known types:\n");
            foreach (var kvp in typeDefinitions)
            {
                sb.AppendFormat("{0}: {1}", kvp.Key, kvp.Value.FullName);
            }

            Debug.Log(sb.ToString());
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
                    if (value.Type == HoconType.Object)
                    {
                        HoconObject obj = value.GetObject();
                        if (obj.ContainsKey("_type"))
                        {
                            // parse as regular object
                            AddConfigObject(value);
                        }
                        else if (obj.ContainsKey("_find"))
                        {
                            // noop: for module manager use
                        }
                        else if (obj.ContainsKey("_conf"))
                        {
                            // parse as global config

                            Debug.Log($"Global configuration added: {obj.ToString()}");
                        }
                        else
                        {
                            Debug.LogWarning($"{path} is not a valid SESim config file.");
                        }
                    }
                    else if (value.Type == HoconType.Array)
                    {
                        foreach (var obj in value.ToArray())
                            AddConfigObject(obj as HoconValue);
                    }
                }
                Debug.Log($"Reading and parsing of {path} completed");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error when reading file \"{path}\"\r\n{e.Message}\r\n{e.StackTrace}");
            }
        }

        private void AddConfigObject(HoconValue value)
        {
            string _type = value.GetObject()["_type"].GetString();

            if (!typeDefinitions.TryGetValue(_type, out var deserializeType))
                throw new Exception($"Type not found when deserializing type \"{_type}\"");

            var ctor = deserializeType.GetConstructor(new Type[0]);
            var deserializeObject = (IHoconDeserializable)ctor.Invoke(new object[0]);
            deserializeObject.ReadFromHocon(value);
            var bag = objects.GetOrAdd(deserializeType, new ConcurrentBag<dynamic>());
            bag.Add(deserializeObject);
        }
    }
    #endregion

}
