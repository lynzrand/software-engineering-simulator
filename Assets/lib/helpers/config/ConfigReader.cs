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
        T ParseHocon(IHoconElement e);
    }

    public interface IHoconDeserializable
    {
        void ReadFromHocon(IHoconElement e);
    }

    public class AnimationCurveParser : IHoconDeserializer<AnimationCurve>
    {
        float ConvFloat(IHoconElement el)
        {
            if (el.Type != HoconType.Literal) throw new ArgumentException();
            var lit = el as HoconLiteral;
            return float.Parse(lit.Value);
        }

        public AnimationCurve ParseHocon(IHoconElement e)
        {
            if (e.Type != HoconType.Array)
                throw new ArgumentException($"The element {e} must be an array");

            var keyFrames = new List<Keyframe>();
            var arr = e as HoconArray;

            foreach (var i in arr)
            {
                if (i.Type != HoconType.Array)
                    throw new ArgumentException($"The element {i} inside an AnimationCurve structure must be an array", nameof(i));
                var iarr = i as HoconArray;
                var farr = iarr.ConvertAll(ConvFloat);
                // key | value | inTangent | outTangent | inWeight | outWeight
                if (farr.Count == 2)
                    keyFrames.Add(new Keyframe(farr[0], farr[1]));
                else if (farr.Count == 4)
                    keyFrames.Add(new Keyframe(farr[0], farr[1], farr[2], farr[3]));
                else if (farr.Count == 6)
                    keyFrames.Add(new Keyframe(farr[0], farr[1], farr[2], farr[3], farr[4], farr[5]));
                else throw new ArgumentException($"{farr} is not a valid keyframe representation");
            }

            return new AnimationCurve(keyFrames.ToArray());

        }
    }
    #endregion

}