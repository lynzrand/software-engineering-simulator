using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sesim.Game.Controllers
{
    public class TargetAnchor : MonoBehaviour
    {
        public string anchorName = "";
    }

    public static class TargetAnchorExtensions
    {
        public static TargetAnchor FindAnchorInChildren(this MonoBehaviour thisObject)
        {
            return thisObject.GetComponentInChildren<TargetAnchor>();
        }

        public static TargetAnchor FindAnchorInChildren(this MonoBehaviour thisObject, string name)
        {
            var anchors = thisObject.GetComponentsInChildren<TargetAnchor>();
            return anchors.FirstOrDefault(anchor => anchor.anchorName == name);
        }

        public static IEnumerable<TargetAnchor> FindAnchorsInChildren(this MonoBehaviour thisObject, string name)
        {
            var anchors = thisObject.GetComponentsInChildren<TargetAnchor>();
            return anchors.TakeWhile(anchor => anchor.anchorName == name);
        }
    }
}
