using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sesim.Models;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Sesim.Game.Controllers.MainGame
{
    public class CompanyActionController : MonoBehaviour
    {
        public static readonly double tickPerSecondAt1x = 60d;
        public static readonly float maxDeltaTPerStep = 10.0f;

        public Company company;
        public Text warpDisplayer;
        public Text timeDisplayer;
        public Camera cam;

        public bool isFocused;

        public float timeWarpMultiplier = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            // Mock up a company before generator and savefile completes
            company = new Company();
            company.Init();
            company.employees.Add(new Employee()
            {
                id = Ulid.NewUlid(),
                name = "Someone",
                baseEfficiency = 1,
                abilities = new Dictionary<string, float>()
                {
                    ["csharp"] = 1.0f
                },
                isWorking = false
            });
            company.contracts.Add(new Contract()
            {
                id = Ulid.NewUlid(),
                name = "Blah blah contract",
                status = ContractStatus.Working,
                members = new List<Employee>() { company.employees[0] },
                timeLimit = 7200 * 16,
                totalWorkload = 30.0,
                completedWork = 0.0,
                techStack = "csharp"
            });
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCompany();

            // var key = Event.current;

            // if (Input.GetKey(KeyCode.A))
            //     cam.transform.Translate(new Vector3(-30f, 0f, 0f) * Time.deltaTime);
            // if (Input.GetKey(KeyCode.W))
            //     cam.transform.Translate(new Vector3(0f, 30f, 0f) * Time.deltaTime);
            // if (Input.GetKey(KeyCode.S))
            //     cam.transform.Translate(new Vector3(0f, -30f, 0f) * Time.deltaTime);
            // if (Input.GetKey(KeyCode.D))
            //     cam.transform.Translate(new Vector3(30f, 0f, 0f) * Time.deltaTime);
        }

        private void UpdateCompany()
        {
            double deltaT = Time.deltaTime * tickPerSecondAt1x * timeWarpMultiplier;
            if (deltaT <= maxDeltaTPerStep)
            {
                company.FixedUpdate(deltaT);
            }
            else
            {
                // If time warp multiplier is too large, we need to split the
                // calculation into multiple iterations to preserve accuracy.
                int iterCount = (int)Math.Ceiling(deltaT / maxDeltaTPerStep);
                for (int i = 0; i < iterCount; i++)
                {
                    company.FixedUpdate(deltaT / iterCount);
                }
            }
            Debug.Log($"time: {UtToTimeString(company.ut)}@{timeWarpMultiplier}x, delta-t: {deltaT.ToString("#.0000")}T, progress: {company.contracts[0].Progress}, status: {company.contracts[0].status}");
        }

        public static string UtToTimeString(double ut)
        {
            var time = UtToTime(ut);
            return String.Format("Day{0:D4} {1:D2}:{2:00.0000}", time.days, time.hours, time.minutes);
        }

        public static (int days, int hours, double minutes) UtToTime(double ut)
        {
            int days = (int)Math.Floor(ut / Company.ticksPerDay);
            int hours = (int)Math.Floor((ut % Company.ticksPerDay) / Company.ticksPerHour);
            double minutes = (ut % Company.ticksPerHour) / (Company.ticksPerHour / 60);

            return (days, hours, minutes);
        }

    }
}
