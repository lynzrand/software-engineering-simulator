using System;
using System.Collections.Generic;
using Ceras;
using UnityEngine;

namespace Sesim.Models
{
    public class Employee
    {
        // Ulids should be able to safely transform into MessagePack
        public Ulid id;

        public string name;

        public float experience = 0.414f;

        public float baseEfficiency;

        public Dictionary<string, float> abilities;

        // TODO: implement
        // public List<Trait> traits;

        public decimal salary;

        public float health = 1.0f;

        public float pressure = 1.0f;

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

        public void SetEfficiencyTimeCurve(
            float startTime = 0.3f, float maxTime = 2f, float declineTime = 6f)
        {
            efficiencyTimeCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f,0f),
                new Keyframe(startTime, 1f),
                new Keyframe(maxTime, 1),
                new Keyframe(declineTime, 0)
            });
        }

        public void SetEfficiencyHealthCurve(
            float oneTangent, float zeroTangent,
            float oneWeight = 0.333f, float zeroWeight = 0.333f)
        {
            efficiencyHealthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f, 0f, 0f, zeroTangent, 0f, zeroWeight),
                new Keyframe(1f, 1f, oneTangent, 0f, oneWeight, 0f)
            });
        }

        public void SetEfficiencyPressureCurve(
            float zeroPressureEfficiency = 0.75f,
            float maxEfficiencyPressure = 0.35f, float maxEfficiency = 1.6f,
            float maxPressureEfficiency = 0.5f)
        {
            efficiencyPressureCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f, zeroPressureEfficiency),
                new Keyframe(maxEfficiencyPressure, maxEfficiency),
                new Keyframe(1f, maxPressureEfficiency)
            });
        }

        public float GetEfficiency(string name, int time)
        {
            if (isWorking && abilities.TryGetValue(name, out float experience))
            {
                var efficiency = baseEfficiency * EfficiencyExperienceMultiplier(experience);
                // TODO: Implement health and pressure features
                // var timeMultiplier = efficiencyTimeCurve.Evaluate((time - lastWorkTime) / 300f);
                // var healthMultiplier = efficiencyHealthCurve.Evaluate(health);
                // var pressureMultiplier = efficiencyPressureCurve.Evaluate(pressure);
                // return efficiency * timeMultiplier * healthMultiplier * pressureMultiplier;
                return efficiency;
            }
            else return 0;
        }

        public static float EfficiencyExperienceMultiplier(float exp)
        {
            return Mathf.Log(exp + 1, 2) + 0.5f;
        }
    }
}
