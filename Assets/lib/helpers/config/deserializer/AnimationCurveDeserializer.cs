using System;
using System.Collections.Generic;
using UnityEngine;
using Hocon;

namespace Sesim.Helpers.Config.Deserializer
{
    public static class AnimationCurveParser
    {

        public static AnimationCurve ParseHocon(HoconValue e)
        {
            if (e.Type != HoconType.Array)
                throw new ArgumentException($"The element {e} must be an array");

            var keyFrames = new List<Keyframe>();
            var arr = e.GetArray();

            foreach (var i in arr)
            {
                if (i.Type != HoconType.Array)
                    throw new ArgumentException($"The element {i} inside an AnimationCurve structure must be an array", nameof(i));
                var iarr = i.GetArray();
                var farr = iarr.ConvertAll((HoconValue v) => v.GetFloat());
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

}