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

        public delegate void AfterCompanyUpdateCallback(CompanyActionController companyController);
        public event AfterCompanyUpdateCallback AfterCompanyUpdate;

        public Company company;
        public Text warpDisplayer;
        public Text timeDisplayer;
        public Camera cam;

        public bool isFocused;

        public float timeWarpMultiplier = 1.0f;

        // Start is called before the first frame update
        public void Start()
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

            if (AfterCompanyUpdate != null) AfterCompanyUpdate.Invoke(this);
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
            var deltaT = Time.deltaTime * tickPerSecondAt1x * timeWarpMultiplier;
            company.Update(deltaT);

            // Log the update before our company is finished
            Debug.Log($"time: {Company.UtToTimeString(company.ut)}@{timeWarpMultiplier}x, fps: {(1 / Time.unscaledDeltaTime).ToString("000.000")}, delta-t: {deltaT.ToString("#.0000")}T, progress: {company.contracts[0].Progress}, status: {company.contracts[0].status}");
        }


    }
}
