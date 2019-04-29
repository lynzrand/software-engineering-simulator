using System;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace Sesim.Models
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class Employee
    {
        public string name;

        public float baseEfficiency;

        public Dictionary<string, float> abilities;

        public List<Trait> traits;

        public decimal salary;

        public float health;

        public float pressure;

        public int lastWorkTime;

        public bool isWorking;

        // This field is reserved for mods
        public Dictionary<string, dynamic> extraData;

        // these two values are for rendering usage
        public Vector3 position;
        public Quaternion rotation;

        // Curves are totally covered by MessagePack so no worries!
        public AnimationCurve efficiencyTimeCurve;
        public AnimationCurve efficiencyHealthCurve;
        public AnimationCurve efficiencyPressureCurve;

        public void setEfficiencyTimeCurve(float startTime, float maxTime, float declineTime)
        {
            efficiencyTimeCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f,0f),
                new Keyframe(startTime, 1f),
                new Keyframe(maxTime, 1),
                new Keyframe(declineTime, 0)
            });
        }

        public void setEfficiencyHealthCurve(
            float oneTangent, float zeroTangent,
            float oneWeight = 0.333f, float zeroWeight = 0.333f)
        {
            efficiencyHealthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f, 0f, 0f, zeroTangent, 0f, zeroWeight),
                new Keyframe(1f, 1f, oneTangent, 0f, oneWeight, 0f)
            });
        }

        public void setEfficiencyPressureCurve(
            float zeroPressureEfficiency,
            float maxEfficiencyPressure, float maxEfficiency,
            float maxPressureEfficiency)
        {
            efficiencyPressureCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f, zeroPressureEfficiency),
                new Keyframe(maxEfficiencyPressure, maxEfficiency),
                new Keyframe(1f, maxPressureEfficiency)
            });
        }

        public float GetEfficiency(string name, int time)
        {
            if (isWorking && abilities.TryGetValue(name, out float multiplier))
            {
                var efficiency = baseEfficiency * multiplier;
                var timeMultiplier = efficiencyTimeCurve.Evaluate((time - lastWorkTime) / 300f);
                var healthMultiplier = efficiencyHealthCurve.Evaluate(health);
                var pressureMultiplier = efficiencyPressureCurve.Evaluate(pressure);
                return efficiency * timeMultiplier * healthMultiplier * pressureMultiplier;
            }
            else return 0;
        }
    }
}