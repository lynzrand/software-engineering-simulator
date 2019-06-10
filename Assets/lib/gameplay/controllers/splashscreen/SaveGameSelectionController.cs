using UnityEngine;
using System;
using Sesim.Models;
using UnityEngine.UI;

namespace Sesim.Game.Controllers.SplashScreen
{
    public class SaveGameSelectionController : MonoBehaviour
    {
        public SaveMetadata metadata;

        public MainMenuController menuController;
        public Text nameText;
        public Text idText;
        public Text timeText;
        public Text employeeCountText;
        public Text contractCountText;

        public ToggleGroup toggleGroup
        {
            get { return GetComponent<Toggle>().group; }
            set { GetComponent<Toggle>().group = value; }
        }

        public void Start()
        {
            this.nameText.text = metadata.name;
            this.idText.text = metadata.id.ToString();
            this.timeText.text = metadata.saveTime.ToLocalTime().ToString();
            this.employeeCountText.text = $"{metadata.employeeCount} EMPLOYEES";
            this.contractCountText.text = $"{metadata.contractCount} CONTRACTS";
            this.GetComponent<Toggle>().onValueChanged.AddListener((bool selected) =>
            {
                menuController.SelectSaveCallback(this.metadata.id, selected);
            });
        }
    }
}
