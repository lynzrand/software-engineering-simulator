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
        
        public Button _btn;
        public int fontSize;
        public Color labelColor;
        public Color contentColor;

        List<Contract> lastContract = new List<Contract>();
        List<Contract> lastAvaliableContract = new List<Contract>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % 48 == 0)
            {
                    if(src.company.contracts != lastContract){
                    lastContract.Clear();
                    lastContract.AddRange(src.company.contracts);

                    var tgtTransform = contractContent.GetComponent<RectTransform>();
                    DestroyAllChildren(tgtTransform) ;

                    foreach (var contract in src.company.contracts)
                    {
                        var gameobject = ConstructGameObj(contract);
                        var transform = gameobject.GetComponent<RectTransform>();
                        transform.SetParent(tgtTransform);
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
