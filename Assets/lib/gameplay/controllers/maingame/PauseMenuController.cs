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
    public class PauseMenuController : MonoBehaviour
    {
        CanvasGroup canvasGroup = null;

        float timeCounter = 0f;

        const float animationTime = 0.35f;

        bool shouldShow = false;

        bool isActive = false;

        public bool ShouldShow
        {
            get => shouldShow; set
            {
                if (value != shouldShow)
                {
                    if (value) Show(); else Hide();
                    shouldShow = value;
                }
            }
        }

        bool isPlayingForward = true;

        public void Start()
        {
            canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        }

        static float Lerp(float a, float b, float progress)
        => (a * (1 - progress)) + (b * (progress));


        public void Show()
        {
            isActive = true;
            isPlayingForward = true;
            timeCounter = 0;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide()
        {
            isActive = true;
            isPlayingForward = false;
            timeCounter = animationTime;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public void Update()
        {
            if (isActive)
            {
                if (isPlayingForward)
                {
                    timeCounter += Time.unscaledDeltaTime;
                    if (timeCounter > animationTime)
                    {
                        timeCounter = animationTime;
                        isActive = false;
                    }
                }
                else
                {
                    timeCounter -= Time.unscaledDeltaTime;
                    if (timeCounter < 0)
                    {
                        timeCounter = 0;
                        isActive = false;
                    }
                }
                canvasGroup.alpha = Lerp(0, 1, timeCounter / animationTime);
            }
        }

        public async void Save()
        {
            await SaveController.Instance.SaveAsync();
        }

        public async void Exit()
        {
            await SaveController.Instance.SaveAsync();
            UnityEngine.Application.Quit();
        }

        public async void ReturnToTitleAsync()
        {
            await SaveController.Instance.SaveAsync();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        }
    }
}
