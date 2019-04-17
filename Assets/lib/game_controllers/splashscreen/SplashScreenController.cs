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
    private ConsoleSize loadingDataSize;

    public Text loadingDataView;

    private bool shouldTransferScene = false;

    // Update is called once per frame
    void Update()
    {
        frames++;
        loadingDataSize = ConsoleHelper.CalculateConsoleSize(loadingDataView);

        loadingDataView.text = formatLoadingText(
            "Testing progressbar",
            $"frames {frames}",
            (float)frames / 500,
            loadingDataSize,
            0.5f
        );
        if (frames > 500) shouldTransferScene = true;
        if (shouldTransferScene) SceneManager.LoadScene("MainGameplayScene");
    }

    string formatLoadingText(string action, string destiniation, float progress, ConsoleSize size,
    float maxActionFillPercent = 0.8f, char cutoffCharacter = '…')
    {
        if (progress < 0) progress = 0; else if (progress > 1) progress = 1;

        StringBuilder sb = new StringBuilder();
        int progressBarSize = size.width - 2;
        int progressBarFilledSize = Mathf.RoundToInt(progressBarSize * progress);
        sb.Append('[');
        sb.Append('|', progressBarFilledSize);
        sb.Append(' ', progressBarSize - progressBarFilledSize);
        sb.Append(']');
        sb.AppendLine();
        int actionMaxSize = Mathf.RoundToInt(size.width * maxActionFillPercent);
        if (action.Length > actionMaxSize)
        {
            action = action.Substring(0, actionMaxSize - 2) + cutoffCharacter;
        }
        int destinationMaxSize = size.width - action.Length - 1;

        if (destiniation.Length > destinationMaxSize - 1)
        {
            destiniation = destiniation.Substring(0, destinationMaxSize - 3) + cutoffCharacter;
        }
        int remainingSize = size.width - action.Length - destiniation.Length;
        sb.Append(action);
        sb.Append(' ', remainingSize);
        sb.Append(destiniation);
        return sb.ToString();
    }
}
