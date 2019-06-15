using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sesim.Game.Controllers;
using Sesim.Models;

namespace Sesim.Game.Controllers.MainGame
{
    public class EmployeeDisplayPanelController : MonoBehaviour
    {
        public CompanyActionController src;
        public Font font;

        public GameObject avaliableEmployeeContent;
        public GameObject employeeContent;

        public Button _btn;
        public Button button;
        public int fontSize;
        public Color labelColor;
        public Color contentColor;

        List<Employee> lastEmployee = new List<Employee>();
        List<Employee> lastAvaliableEmployee = new List<Employee>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % 48 == 0)
            {
                if(src.company.employees != lastEmployee){
                    lastEmployee.Clear();
                    lastEmployee.AddRange(src.company.employees);

                    var panelContent = employeeContent.GetComponent<RectTransform>();
                    DestroyAllChildren(panelContent) ;

                    foreach (var employee in src.company.employees)
                    {
                        var gameobject = ConstructGameObj(employee);
                        var transform = gameobject.GetComponent<RectTransform>();

                        var btn = new GameObject("_btn",    typeof(Button), typeof(Text));
                        _btn =  btn.GetComponent<Button>();
                        _btn.onClick.AddListener(delegate(){
                            this.Fire(employee);
                        });
                        
                        var btnText = btn.GetComponent<Text>();
                        btnText.text = "[ Fire ]";
                        btnText.font = font;
                        btnText.fontSize = fontSize;
                        btnText.color = contentColor;      

                        btn.transform.SetParent(panelContent, false);

                        transform.SetParent(panelContent);
                        transform.localScale = Vector3.one;
                    }
                }

                if(src.company.avaliableEmployees != lastAvaliableEmployee){
                    lastAvaliableEmployee.Clear();
                    lastAvaliableEmployee.AddRange(src.company.avaliableEmployees);

                    var tgtTransform2 = avaliableEmployeeContent.GetComponent<RectTransform>();
                    DestroyAllChildren(tgtTransform2);
                    // var transform = gameobject.GetComponent<RectTransform>();
                    foreach (var employee in src.company.avaliableEmployees)
                    {
                        var gameobject = ConstructGameObj2(employee);
                        var transform = gameobject.GetComponent<RectTransform>();
                        
                        var btn = new GameObject("_btn",    typeof(Button), typeof(Text));

                        _btn =  btn.GetComponent<Button>();
                        // _btn.onClick.AddListener(Show);
                        _btn.onClick.AddListener(delegate(){
                            this.Hire(employee);
                        });
                        // _btn.onClick.RemoveListener(Show);
                        
                        var btnText = btn.GetComponent<Text>();
                        btnText.text = "[ Hire ]";
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

        void Hire(Employee e)
        {
            src.company.AddEmployee(e);
        }

        void Fire(Employee e)
        {
            src.company.RemoveEmployee(e.id);
        }
        GameObject ConstructGameObj(Employee e)
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

        GameObject ConstructGameObj2(Employee e)
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
            kvdr.Construct(EmployeeToKvde2(e));

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
                new KeyValueDisplayElement("isEmployeed", e.isEmployeed.ToString()),
            };
        }

        IList<KeyValueDisplayElement> EmployeeToKvde2(Employee e)
        {
            return new List<KeyValueDisplayElement>()
            {
                new KeyValueDisplayElement("salary", e.salary.ToString()),
                new KeyValueDisplayElement("experience", e.experience.ToString()),
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
