using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sesim.Helpers.UI;
using System.Text;

namespace Sesim.Game.Controllers
{
    public class SplashScreenController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        private int frames = 0;
        private Vector2Int loadingDataSize;

        public Text loadingDataView;
        public Text versionText;
        public Animator animator;

        private bool shouldTransferScene = false;
        private bool loaded = false;

        // Update is called once per frame
        void Update()
        {
            if (!loaded) frames++;

            loadingDataSize = ConsoleHelper.GetConsoleSize(loadingDataView);
            loadingDataView.text = FormatLoadingText(
                "Testing progressbar",
                $"frames {frames}",
                (float)frames / 500,
                loadingDataSize
            );
            if (frames > 500) LoadedHandler();
            // if (shouldTransferScene) SceneManager.LoadScene("MainGameplayScene");
        }

        string FormatLoadingText(string action, string destination, float progress, Vector2Int size)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConsoleHelper.GenerateProgressBar(size.x, progress));
            sb.AppendLine();
            sb.Append(ConsoleHelper.GenerateTrimmedActionDescription(size.x, action, destination));
            return sb.ToString();
        }

        public void LoadedHandler()
        {
            //  call showMenu()
            loaded = true;
            animator.SetBool("Loaded", true);

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