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

        public float experience = 0f;

        public float baseEfficiency = 1f;

        public Dictionary<string, float> abilities;

        // TODO: implement
        // public List<Trait> traits;

        public decimal salary = 0;

        public float health = 1.0f;

        public float pressure = 1.0f;

        public double lastWorkTime = 0;

        public bool isWorking = true;

        // This field is reserved for mods
        public Dictionary<string, dynamic> extraData;

        // these two values are for rendering usage
        public Vector3 position;
        public Quaternion rotation;

        // Curves are totally covered by Ceras so no worries!
        public AnimationCurve efficiencyTimeCurve;
        public AnimationCurve efficiencyHealthCurve;
        public AnimationCurve efficiencyPressureCurve;

        public Employee()
        {
            SetEfficiencyHealthCurve();
            SetEfficiencyPressureCurve();
            SetEfficiencyTimeCurve();
        }

        public void SetEfficiencyTimeCurve(
            float startEfficiency = 0.5f, float startTime = 0.3f,
            float maxTime = 2f, float declineTime = 6f)
        {
            efficiencyTimeCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f, startEfficiency),
                new Keyframe(startTime, 1f),
                new Keyframe(maxTime, 1f),
                new Keyframe(declineTime, 0f)
            });
        }

        public void SetEfficiencyHealthCurve(
            float oneTangent = 0f, float zeroTangent = 0f,
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

        public float GetEfficiency(string techStackName, double ut,
            bool useTime = true, bool useHealth = true, bool usePressure = true)
        {
            if (isWorking && abilities.TryGetValue(techStackName, out float experience))
            {
                var efficiency = baseEfficiency * EfficiencyExperienceMultiplier(this.experience) * EfficiencyAbilityMultiplier(experience);

                var timeMultiplier = useTime
                    ? efficiencyTimeCurve?.Evaluate((float)(ut - lastWorkTime) / 300) ?? 1f
                    : 1f;

                var healthMultiplier = useHealth
                    ? efficiencyHealthCurve?.Evaluate(health) ?? 1f
                    : 1f;

                var pressureMultiplier = usePressure
                    ? efficiencyPressureCurve?.Evaluate(pressure) ?? 1f
                    : 1f;

                return efficiency * timeMultiplier * healthMultiplier * pressureMultiplier;
                // return efficiency;
            }
            else return 0;
        }

        /// <summary>
        /// Update working status for this employee (and track work start time)
        /// </summary>
        /// <param name="ut"></param>
        /// <param name="shouldWork"></param>
        public void UpdateWorkStatus(double ut, bool shouldWork)
        {
            if (shouldWork)
            {
                if (!isWorking)
                {
                    isWorking = true;
                    lastWorkTime = ut;
                }
            }
            else
            {
                isWorking = false;
            }
        }

        public static float EfficiencyExperienceMultiplier(float exp)
        {
            return Mathf.Log(exp + 2, 2);
        }

        public static float EfficiencyAbilityMultiplier(float exp)
        {
            return Mathf.Log(exp + 1, 2);
        }
    }
}
