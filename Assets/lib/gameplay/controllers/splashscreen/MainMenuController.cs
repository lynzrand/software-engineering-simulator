using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sesim.Helpers.UI;
using System.Text;
using Sesim.Models;
using Sesim.Game.Controllers.Persistent;

namespace Sesim.Game.Controllers.SplashScreen
{
    public class MainMenuController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        private int frames = 0;
        private Vector2Int loadingDataSize;

        public Text versionText;

        public GameObject loadGamePanel;
        public GameObject SaveGameSelectionPrefab;
        private bool loaded = false;

        private Dictionary<Ulid, bool> saveSelection = new Dictionary<Ulid, bool>();

        void Awake()
        {
            this.versionText.text = $"SESim {Application.version}";
        }

        public void NewGameHandler()
        {
            SaveController.Instance.shouldCompanyLoadSavefile = false;
            SceneManager.LoadScene("MainGameplayScene");
        }

        public void LoadGameHandler()
        {
            loadGamePanel.SetActive(!loadGamePanel.activeSelf);
            InitializeLoadGamePanel();
        }

        public void InitializeLoadGamePanel()
        {
            var metas = SaveController.Instance.loadSaveMetas();
            var panelContent = loadGamePanel.GetComponentInChildren<TargetAnchor>().gameObject;

            DestroyAllChildren(panelContent.transform);
            this.saveSelection.Clear();

            ToggleGroup toggleGroup = panelContent.GetComponent<ToggleGroup>();

            foreach (var meta in metas)
            {
                var option = ConstructSingleLoadGameOption(meta);

                var saveGameSelectionController = option.GetComponent<SaveGameSelectionController>();

                saveGameSelectionController.toggleGroup = toggleGroup;
                saveGameSelectionController.menuController = this;

                option.transform.SetParent(panelContent.transform, false);
            }
        }

        GameObject ConstructSingleLoadGameOption(SaveMetadata meta)
        {
            var rootGameobject = Instantiate(SaveGameSelectionPrefab);
            var controller = rootGameobject.GetComponent<SaveGameSelectionController>();
            controller.metadata = meta;
            controller.Start();
            return rootGameobject;
        }

        public void SelectSaveCallback(Ulid id, bool selected)
        {
            saveSelection[id] = selected;
        }

        (bool, Ulid) EvaluateSaveSelection()
        {
            Ulid id = new Ulid();
            bool any = false;
            foreach (var kvp in this.saveSelection)
            {
                if (kvp.Value)
                {
                    any = true;
                    id = kvp.Key;
                }
            }
            return (any, id);
        }

        public void LoadSelectedGameHandler()
        {
            (bool any, Ulid id) = EvaluateSaveSelection();
            if (any)
            {
                var saveController = SaveController.Instance;

                saveController.Load(id);
                SceneManager.LoadScene("MainGameplayScene");
            }
        }

        public void CloseLoadGamePanelHandler()
        {
            loadGamePanel.SetActive(false);
        }

        public void SettingsHandler()
        {
            Debug.LogError("Settings is not avaliable for now. Nothing is settable.");
        }

        public void GameExitHandler()
        {
            UnityEngine.Application.Quit();
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
