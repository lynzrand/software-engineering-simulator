using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sesim.Helpers.UI;
using System.Text;

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

        private bool shouldTransferScene = false;
        private bool loaded = false;

        void Awake()
        {
            this.versionText.text = $"SESim {Application.version}";
        }

        public void NewGameHandler()
        {
            SceneManager.LoadScene("MainGameplayScene");
        }

        public void LoadGameHandler()
        {
            SceneManager.LoadScene("MainGameplayScene");
        }

        public void SettingsHandler() { }

        public void GameExitHandler()
        {
            UnityEngine.Application.Quit();
        }
    }

    public class SplashScreenAnimationController : Animator
    {

    }
}
