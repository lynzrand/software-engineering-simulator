using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Sesim.Helpers.UI;
using System.Text;

namespace Sesim.Game.Controllers
{
    public class SplashScreenController : MonoBehaviour
    {
        private Vector2Int consoleSize;
        public Text console;
        float uiScale = 1;

        public Base16ColorScheme scheme = Base16ColorScheme.MaterialPaleNight;

        // 500 lines should be more than enough
        private List<string> cache = new List<string>(500);

        public void Start()
        {
            addLog();
        }

        int counter = 0;
        int maxCounter = 1024;

        void addLog()
        {
            Application.logMessageReceived += logOntoConsole;
        }

        void removeLog()
        {
            Application.logMessageReceived -= logOntoConsole;
        }

        public void logOntoConsole(string message, string trace, LogType type)
        {
            var timeStr = DateTime.Now.ToString("HH:mm:ss.fffffff");
            var typeRepr = type.ToString().ToUpper();
            switch (type)
            {
                case LogType.Assert:
                    typeRepr = ApplyColor(typeRepr, scheme.base0F);
                    break;
                case LogType.Error:
                    typeRepr = ApplyColor(typeRepr, scheme.base08);
                    break;
                case LogType.Exception:
                    typeRepr = ApplyColor(typeRepr, scheme.base09);
                    break;
                case LogType.Warning:
                    typeRepr = ApplyColor(typeRepr, scheme.base0A);
                    break;
                case LogType.Log:
                    typeRepr = ApplyColor(typeRepr, scheme.base0C);
                    break;
            }
            cache.Add($"<b>[{timeStr}] [{typeRepr}]</b> {message}");
        }

        public static string ColorToString(Color32 color)
        {
            return $"#{color.r.ToString("X2")}{color.g.ToString("X2")}{color.b.ToString("X2")}{color.a.ToString("X2")}";
        }
        public static string ApplyColor(string str, Color32 color)
        {
            return $"<color={ColorToString(color)}>{str}</color>";
        }

        public void Update()
        {
            uiScale = console.canvas.scaleFactor * 3 / 4;
            counter++;
            if (counter > maxCounter) counter = 0;
            consoleSize = ConsoleHelper.GetConsoleSize(console, uiScale);
            Debug.Log(counter);
            Debug.LogWarning("Warning test");
            Debug.LogError("meow!");
            Debug.LogAssertion("Assert failed!");
            var sb = new StringBuilder();
            for (int i = Math.Max(0, cache.Count - consoleSize.y); i < cache.Count; i++)
            {
                sb.AppendLine(cache[i]);
            }
            sb.Append(ConsoleHelper.GenerateProgressBar(consoleSize.x, (float)counter / maxCounter,
            filled: '#', empty: '-', filledCap: '#'));
            console.text = sb.ToString();
        }
    }
}