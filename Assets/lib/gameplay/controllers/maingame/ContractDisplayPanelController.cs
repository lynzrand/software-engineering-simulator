using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sesim.Game.Controllers;
using Sesim.Models;

namespace Sesim.Game.Controllers.MainGame
{
    public class ContractDisplayPanelController : MonoBehaviour
    {
        public CompanyActionController src;
        public Font font;

        public GameObject avaliableContractContent;
        public GameObject contractContent;

        public GameObject employeeContent;

        public GameObject statusPanel;
        
        public Button _btn;
        public Button button;
        public int fontSize;
        public Color labelColor;
        public Color contentColor;

        List<Contract> lastContract = new List<Contract>();
        List<Contract> lastAvaliableContract = new List<Contract>();

        // Start is called before the first frame update
        void Start()
        {
            statusPanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % 48 == 0)
            {
                if(src.company.contracts != lastContract){
                    lastContract.Clear();
                    lastContract.AddRange(src.company.contracts);

                    var panelContent = contractContent.GetComponent<RectTransform>();
                    DestroyAllChildren(panelContent) ;

                    foreach (var contract in src.company.contracts)
                    {
                        var gameobject = ConstructGameObj(contract);
                        var transform = gameobject.GetComponent<RectTransform>();

                        var btn = new GameObject("_btn",    typeof(Button), typeof(Text));
                        var Ebtn = new GameObject("_btn",    typeof(Button), typeof(Text));

                        _btn =  btn.GetComponent<Button>();
                        // _btn.onClick.AddListener(Show);
                        _btn.onClick.AddListener(delegate(){
                            this.Abandon(contract);
                        });
                        // _btn.onClick.RemoveListener(Show);
                        button =  Ebtn.GetComponent<Button>();
                        button.onClick.AddListener(delegate(){
                            this.assign(contract);
                        });
                        
                        var btnText = btn.GetComponent<Text>();
                        btnText.text = "[ Abandon ]";
                        btnText.font = font;
                        btnText.fontSize = fontSize;
                        btnText.color = contentColor;      

                        var EbtnText = Ebtn.GetComponent<Text>();
                        EbtnText.text = "[ Assign ]";
                        EbtnText.font = font;
                        EbtnText.fontSize = fontSize;
                        EbtnText.color = contentColor;

                        btn.transform.SetParent(panelContent, false);
                        Ebtn.transform.SetParent(panelContent, false);

                        transform.SetParent(panelContent);
                        transform.localScale = Vector3.one;
                    }
                }

                if(src.company.avaliableContracts != lastAvaliableContract){
                    lastAvaliableContract.Clear();
                    lastAvaliableContract.AddRange(src.company.avaliableContracts);

                    var tgtTransform2 = avaliableContractContent.GetComponent<RectTransform>();
                    DestroyAllChildren(tgtTransform2);
                    // var transform = gameobject.GetComponent<RectTransform>();
                    foreach (var contract in src.company.avaliableContracts)
                    {
                        var gameobject = ConstructGameObj2(contract);
                        var transform = gameobject.GetComponent<RectTransform>();
                        
                        var btn = new GameObject("_btn",    typeof(Button), typeof(Text));

                        _btn =  btn.GetComponent<Button>();
                        // _btn.onClick.AddListener(Show);
                        _btn.onClick.AddListener(delegate(){
                            this.Accept(contract);
                        });
                        // _btn.onClick.RemoveListener(Show);
                        
                        var btnText = btn.GetComponent<Text>();
                        btnText.text = "[ Accept ]";
                        btnText.font = font;
                        btnText.fontSize = fontSize;
                        btnText.color = contentColor;

                        btn.transform.SetParent(tgtTransform2, false);
                        transform.SetParent(tgtTransform2);
                        transform.localScale = Vector3.one;
                    }
                }        
            }
            
        }

        // void Show()
        // {
        //     print("hello");
        // }

        void Accept(Contract c)
        {
            src.company.AddContract(c);
            // statusPanel.SetActive(true);
        }

        void Abandon(Contract c)
        {
            src.company.RemoveContract(c.id);
        }

        void assign(Contract c)
        {
            statusPanel.SetActive(true);
            InitializeStatusPanel(c);
        }
        public void InitializeStatusPanel(Contract c)
        {
            var panelContent = statusPanel.GetComponentInChildren<TargetAnchor>().gameObject;
            DestroyAllChildren(panelContent.transform);

            var btn = new GameObject("_btn",    typeof(Button), typeof(Text));
            _btn =  btn.GetComponent<Button>();
            _btn.onClick.AddListener(CloseStatusPanelHandler);
            var btnText = btn.GetComponent<Text>();
            btnText.text = "[ Close ]";
            btnText.font = font;
            btnText.fontSize = fontSize;
            btnText.color = contentColor;

            btn.transform.SetParent(panelContent.transform, false);

            foreach (var Employee in src.company.employees)
            {
                if(Employee.isWorking == false)
                {
                    var gameobject = EmployeeConstructGameObj(Employee);
                    var transform = gameobject.GetComponent<RectTransform>();

                    var Ebtn = new GameObject("_btn",    typeof(Button), typeof(Text));
                    button =  Ebtn.GetComponent<Button>();
                    button.onClick.AddListener(delegate(){
                        this.assignEmployee(Employee,c);
                    });

                    var EbtnText = Ebtn.GetComponent<Text>();
                    EbtnText.text = "[ Assign ]";
                    EbtnText.font = font;
                    EbtnText.fontSize = fontSize;
                    EbtnText.color = contentColor;

                    btn.transform.SetParent(panelContent.transform, false);
                    Ebtn.transform.SetParent(panelContent.transform, false);

                    transform.SetParent(panelContent.transform);
                    transform.localScale = Vector3.one;
                }   
            }
        }
        public void CloseStatusPanelHandler()
        {
            statusPanel.SetActive(false);
        }

        public void assignEmployee(Employee e,Contract c)
        {     
            c.members.Add(e);
            e.isWorking = true;
            CloseStatusPanelHandler();
            InitializeStatusPanel(c);
        }

        GameObject ConstructGameObj(Contract c)
        {
            var rootGameobject = new GameObject($"{c.name}_Obj", typeof(RectTransform), typeof(KeyValueDisplayer), typeof(VerticalLayoutGroup), typeof(LayoutElement));


            var vlg = rootGameobject.GetComponent<VerticalLayoutGroup>();
            vlg.childForceExpandWidth = true;
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.spacing = 2f;

            var kvdr = rootGameobject.GetComponent<KeyValueDisplayer>();
            kvdr.title = c.name;
            kvdr.font = this.font;
            kvdr.fontSize = this.fontSize;
            kvdr.labelColor = this.labelColor;
            kvdr.contentColor = this.contentColor;
            kvdr.Construct(ContractToKvde(c));

            return rootGameobject;
        }

        GameObject ConstructGameObj2(Contract c)
        {
            var rootGameobject = new GameObject($"{c.name}_Obj", typeof(RectTransform), typeof(KeyValueDisplayer), typeof(VerticalLayoutGroup), typeof(LayoutElement));


            var vlg = rootGameobject.GetComponent<VerticalLayoutGroup>();
            vlg.childForceExpandWidth = true;
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.spacing = 2f;

            var kvdr = rootGameobject.GetComponent<KeyValueDisplayer>();
            kvdr.title = c.name;
            kvdr.font = this.font;
            kvdr.fontSize = this.fontSize;
            kvdr.labelColor = this.labelColor;
            kvdr.contentColor = this.contentColor;
            kvdr.Construct(ContractToKvde2(c)); 

            return rootGameobject;
        }

        GameObject EmployeeConstructGameObj(Employee e)
        {
            var rootGameobject = new GameObject($"{e.name}_Obj", typeof(RectTransform), typeof(KeyValueDisplayer), typeof(VerticalLayoutGroup), typeof(LayoutElement));

            var vlg = rootGameobject.GetComponent<VerticalLayoutGroup>();
            vlg.childForceExpandWidth = true;
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.spacing = 2f;

            var kvdr = rootGameobject.GetComponent<KeyValueDisplayer>();
            kvdr.title = e.name;
            kvdr.font = this.font;
            kvdr.fontSize = this.fontSize;
            kvdr.labelColor = this.labelColor;
            kvdr.contentColor = this.contentColor;
            kvdr.Construct(EmployeeToKvde(e));

            return rootGameobject;
        }
        IList<KeyValueDisplayElement> EmployeeToKvde(Employee e)
        {
            return new List<KeyValueDisplayElement>()
            {
                new KeyValueDisplayElement("salary", e.salary.ToString()),
                new KeyValueDisplayElement("experience", e.experience.ToString()),
                new KeyValueDisplayElement("health", e.health.ToString()),
                new KeyValueDisplayElement("pressure", e.pressure.ToString()),
            };
        }

        IList<KeyValueDisplayElement> ContractToKvde(Contract c)
        {
            return new List<KeyValueDisplayElement>()
            {
                new KeyValueDisplayElement("status", c.status.ToString()),
                new KeyValueDisplayElement("timeLimit", c.timeLimit.ToString()),
                new KeyValueDisplayElement("totalWorkload", c.totalWorkload.ToString()),
                new KeyValueDisplayElement("Process", (c.completedWork/c.totalWorkload).ToString()),
                new KeyValueDisplayElement("fund", c.completeReward.fund.ToString()),
                new KeyValueDisplayElement("reputation", c.completeReward.reputation.ToString()),
            };
        }

        IList<KeyValueDisplayElement> ContractToKvde2(Contract c)
        {
            return new List<KeyValueDisplayElement>()
            {
                new KeyValueDisplayElement("status", c.status.ToString()),  
                new KeyValueDisplayElement("contractor", c.contractor.ToString()),
                new KeyValueDisplayElement("description", c.description.ToString()),
                new KeyValueDisplayElement("startTime", Company.UtToTimeString(c.startTime)),
                new KeyValueDisplayElement("totalWorkload", c.totalWorkload.ToString()),
            };
        }

        static void DestroyAllChildren(Transform src)
        {
            var childCount = src.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var childZero = src.GetChild(0);
                childZero.SetParent(null);
                Destroy(childZero.gameObject);
            }
        }
    }
}
