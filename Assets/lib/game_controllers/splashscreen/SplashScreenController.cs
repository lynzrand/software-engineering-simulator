using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sesim.Helpers.UI;
using System.Text;

public class SplashScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private int frames = 0;
    private Vector2Int loadingDataSize;

    public Text loadingDataView;

    private bool shouldTransferScene = false;

    // Update is called once per frame
    void Update()
    {
        frames++;

        loadingDataSize = ConsoleHelper.GetConsoleSize(loadingDataView);
        loadingDataView.text = formatLoadingText(
            "Testing progressbar",
            $"frames {frames}",
            (float)frames / 500,
            loadingDataSize
        );
        if (frames > 500) shouldTransferScene = true;
        if (shouldTransferScene) SceneManager.LoadScene("MainGameplayScene");
    }

    string formatLoadingText(string action, string destination, float progress, Vector2Int size)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(ConsoleHelper.GenerateProgressBar(size.x, progress));
        sb.AppendLine();
        sb.Append(ConsoleHelper.GenerateTrimmedActionDescription(size.x, action, destination));
        return sb.ToString();
    }
}
