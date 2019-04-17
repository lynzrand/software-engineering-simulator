using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sesim.Helpers.UI;

public class SplashScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        loadingDataSize = ConsoleHelper.CalculateConsoleSize(loadingDataView);
    }

    private int frames = 0;
    private ConsoleSize loadingDataSize;

    public Text loadingDataView;

    private bool shouldTransferScene = false;

    // Update is called once per frame
    void Update()
    {
        frames++;
        loadingDataView.text = $"Frame {frames}".PadLeft(loadingDataSize.width, '=');
        if (frames > 500) shouldTransferScene = true;
        if (shouldTransferScene) SceneManager.LoadScene("MainGameplayScene");
    }
}
