using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Hocon;
using System.Threading.Tasks;

namespace Sesim.Helpers.Config
{
    #region readers
    public interface IConfDeserializable
    {
        void DeserializeFromHocon(IHoconElement rootNode);
    }

    public class ConfigReader
    {
        public string root;

        public Queue<string> directoriesToScan = new Queue<string>();

        public Dictionary<string, Type> typeDefinitions { get; }

        public List<Dictionary<string, dynamic>> objects;

        public void AssignType(string typeAnnotation, Type type)
        {
            if (type.GetInterface("IConfDeserializable") != null)
                typeDefinitions[typeAnnotation] = type;
            else
                throw new InvalidDataException($"{type.FullName} does not implement IConfDeserializable.");
        }

        async public Task TraverseDirectoriesAsync(string dir)
        {
            var files = Directory.EnumerateFiles(dir);
            foreach (var file in files)
            {
                await ReadConfigFile(file);
            }
            var subDirs = Directory.EnumerateDirectories(dir);
            foreach (var subdir in subDirs)
            {
                await TraverseDirectoriesAsync(subdir);
            }
        }

        async Task ReadConfigFile(string path)
        {
            using (var f = File.OpenText(path))
            {
                var str = await f.ReadToEndAsync();
            }
        }
    }
    #endregion


    #region parsers
    public interface IHoconDeserializer<T>
    {
        Type ExpectedType { get; }
        T ParseHocon(HoconValue e);
    }

    public interface IHoconDeserializable
    {
        void ReadFromHocon(HoconValue e);
    }

    #endregion

}