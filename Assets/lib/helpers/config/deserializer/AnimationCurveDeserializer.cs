using System;
using System.Collections.Generic;
using UnityEngine;
using Hocon;

namespace Sesim.Helpers.Config.Deserializer
{
    public class AnimationCurveParser : IHoconDeserializer<AnimationCurve>
    {
        public Type ExpectedType => typeof(AnimationCurve);

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

}