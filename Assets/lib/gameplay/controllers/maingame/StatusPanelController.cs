using System;
using Sesim.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Sesim.Game.Controllers.MainGame
{
    public class StatusPanelController : MonoBehaviour
    {
        public CompanyActionController parentController;
        public Text timeDisplayer;
        public Text warpDisplayer;
        public Text fundDisplayer;
        public Text reputationDisplayer;

        public void Start()
        {
            parentController = parentController ?? gameObject.GetComponentInParent<CompanyActionController>();
            parentController.AfterCompanyUpdate += this.PassiveUpdate;
        }

        public void PassiveUpdate(CompanyActionController controller)
        {
            timeDisplayer.text = Company.UtToTimeString(parentController.company.ut);
        }
    }
}
