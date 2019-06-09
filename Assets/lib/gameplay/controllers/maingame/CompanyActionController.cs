using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sesim.Models;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Sesim.Game.Controllers.Persistent;

namespace Sesim.Game.Controllers.MainGame
{
    public class CompanyActionController : MonoBehaviour
    {
        public static readonly double tickPerSecondAt1x = 60d;

        public delegate void AfterCompanyUpdateCallback(CompanyActionController companyController);
        public event AfterCompanyUpdateCallback AfterCompanyUpdate;

        public PauseMenuController pauseMenuController;

        public Company company;

        public SaveFile saveFile;
        public Camera cam;

        public bool isFocused;

        public float timeWarpMultiplier = 1.0f;

        public bool isPaused = false;

        // Start is called before the first frame update
        public void Awake()
        {
            // Mock up a company before generator and savefile completes
            SaveController saveController = SaveController.Instance;
            if (saveController.shouldCompanyLoadSavefile)
            {
                this.saveFile = saveController.saveFile;
                this.company = saveController.saveFile.company;
                saveController.shouldCompanyLoadSavefile = false;
            }
            else
            {
                saveFile = new SaveFile()
                {
                    id = Ulid.NewUlid(),
                    name = "new save",
                };
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
                    completeReward = new ContractReward()
                    {
                        fund = 10000,
                        reputation = 10
                    },
                    techStack = "csharp"
                });
                company.contracts.Add(new Contract()
                {
                    id = Ulid.NewUlid(),
                    name = "Blah blah contract 2",
                    status = ContractStatus.Working,
                    members = new List<Employee>() { company.employees[0] },
                    timeLimit = 7200 * 16,
                    totalWorkload = 30.0,
                    completedWork = 0.0,
                    completeReward = new ContractReward()
                    {
                        fund = 10000,
                        reputation = 10
                    },
                    techStack = "csharp"
                });
                saveFile.company = company;
                saveController.saveFile = saveFile;
            }
            company.contractFactories.Add(new ContractFactory().SetDebugDefault());
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPaused)
            {
                UpdateCompany();
                if (AfterCompanyUpdate != null)
                    AfterCompanyUpdate.Invoke(this);
            }

            var evSys = EventSystem.current;
            if (evSys.currentSelectedGameObject == null)
                evSys.SetSelectedGameObject(this.gameObject);
            var isActive = evSys.currentSelectedGameObject == this.gameObject;
            // Debug.Log(evSys.currentSelectedGameObject?.name);
            if (isActive)
            {
                if (Input.GetKeyDown(KeyCode.Period))
                {
                    timeWarpMultiplier *= 2;
                }
                if (Input.GetKeyDown(KeyCode.Comma))
                {
                    timeWarpMultiplier /= 2;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    timeWarpMultiplier = 1;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isPaused = !isPaused;
                }

                timeWarpMultiplier = Mathf.Clamp(timeWarpMultiplier, 1, 128);
            }
            pauseMenuController.ShouldShow = isPaused;

            CheckKeys();
            // if (Input.GetAxis(""))
        }

        private Vector3 lastMousePos;

        private void CheckKeys()
        {
            // pan on wasd
            if (Input.GetKey(KeyCode.A))
                cam.transform.Translate(new Vector3(-30f, 0f, 0f) * Time.deltaTime);
            if (Input.GetKey(KeyCode.W))
                cam.transform.Translate(new Vector3(0f, 30f, 0f) * Time.deltaTime);
            if (Input.GetKey(KeyCode.S))
                cam.transform.Translate(new Vector3(0f, -30f, 0f) * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                cam.transform.Translate(new Vector3(30f, 0f, 0f) * Time.deltaTime);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Pan on right or middle drag
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    lastMousePos = Input.mousePosition;
                }

                if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
                {
                    var mouseMove = lastMousePos - Input.mousePosition;

                    var cameraForward = cam.transform.TransformDirection(Vector3.forward);
                    cameraForward.y = 0;
                    cameraForward = cameraForward.normalized;

                    var cameraRight = cam.transform.TransformDirection(Vector3.right);
                    cameraRight.y = 0;
                    cameraRight = cameraRight.normalized;

                    var multiplier = 0.003f * cam.orthographicSize;

                    var moveDelta =
                        mouseMove.y * cameraForward * multiplier
                        + mouseMove.x * cameraRight * multiplier;

                    cam.transform.Translate(moveDelta, Space.World);
                    lastMousePos = Input.mousePosition;
                }

                {
                    // zoom on scroll
                    const float sizeDeltaMult = 0.1f;
                    cam.orthographicSize *= Mathf.Exp(-Input.mouseScrollDelta.y * sizeDeltaMult);
                }
            }
        }

        private void UpdateCompany()
        {
            var deltaT = Time.deltaTime * tickPerSecondAt1x * timeWarpMultiplier;
            company.Update(deltaT);

            // Log the update before our company is finished
            // Debug.Log($"time: {Company.UtToTimeString(company.ut)}@{timeWarpMultiplier}x, fps: {(1 / Time.unscaledDeltaTime).ToString("000.000")}, delta-t: {deltaT.ToString("#.0000")}T, progress: {company.contracts[0].Progress}, status: {company.contracts[0].status}");
        }


    }
}
