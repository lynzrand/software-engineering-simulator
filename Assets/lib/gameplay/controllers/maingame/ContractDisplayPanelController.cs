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

        GameObject content;

        public int fontSize;
        public Color labelColor;
        public Color contentColor;

        // Start is called before the first frame update
        void Start()
        {
            content = this.FindAnchorInChildren().gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            var tgtTransform = content.gameObject.GetComponent<RectTransform>();
            DestroyAllChildren(tgtTransform);

            foreach (var contract in src.company.contracts)
            {
                var gameobject = ConstructGameObj(contract);
                var transform = gameobject.GetComponent<RectTransform>();
                transform.SetParent(tgtTransform);
                transform.localScale = Vector3.one;
            }
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

        IList<KeyValueDisplayElement> ContractToKvde(Contract c)
        {
            return new List<KeyValueDisplayElement>()
            {
                new KeyValueDisplayElement("Progress", c.Progress.ToString("0.000000")),
                new KeyValueDisplayElement("Employee", c.members.Count.ToString()),
            };
        }

        static void DestroyAllChildren(Transform src)
        {
            var childCount = src.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var childZero = src.GetChild(i);
                childZero.SetParent(null);
                Destroy(childZero.gameObject);
            }
        }
    }
}
