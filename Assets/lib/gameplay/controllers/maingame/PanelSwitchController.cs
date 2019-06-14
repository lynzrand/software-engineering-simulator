using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sesim.Models;
using Sesim.Game.Controllers.Persistent;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Sesim.Game.Controllers.MainGame
{
    public class PanelSwitchController : MonoBehaviour
    {
        public GameObject panelRoot;

        public GameObject buttonsRoot;

        public ToggleGroup group;

        int activePanel = 0;

        public void Start()
        {
            var count = buttonsRoot.transform.childCount;
            for (var i = 0; i < count; i++)
            {
                var component = buttonsRoot.transform.GetChild(i);
                var toggle = component.GetComponent<Toggle>();
                int j = i;
                toggle.onValueChanged.AddListener((bool active) =>
                {
                    if (active)
                    {
                        setActivePanel(j);
                    }
                });
            }
        }

        public void setActivePanel(int index)
        {
            if (index >= panelRoot.transform.childCount) throw new Exception($"{index}, {panelRoot.transform.childCount}");
            activePanel = index;
            disableAllPanelChildren();
            panelRoot.transform.GetChild(index).gameObject.SetActive(true);
        }

        void disableAllPanelChildren()
        {
            Transform panelTransform = panelRoot.transform;
            var childCount = panelTransform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                panelTransform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
