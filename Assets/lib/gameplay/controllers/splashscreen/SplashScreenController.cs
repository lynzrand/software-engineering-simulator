using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Sesim.Helpers.UI;
using System.Text;

namespace Sesim.Game.Controllers.SplashScreen
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
                    typeRepr = ConsoleHelper.ApplyColor(typeRepr, scheme.base0F);
                    break;
                case LogType.Error:
                    typeRepr = ConsoleHelper.ApplyColor(typeRepr, scheme.base08);
                    break;
                case LogType.Exception:
                    typeRepr = ConsoleHelper.ApplyColor(typeRepr, scheme.base09);
                    break;
                case LogType.Warning:
                    typeRepr = ConsoleHelper.ApplyColor(typeRepr, scheme.base0A);
                    break;
                case LogType.Log:
                    typeRepr = ConsoleHelper.ApplyColor(typeRepr, scheme.base0C);
                    break;
            }
            cache.Add($"<b>[{timeStr}] [{typeRepr}]</b> {message}");
        }



        public void Update()
        {
            var screenDpi = Screen.dpi;
            uiScale = screenDpi / 96;

            counter++;
            if (counter > maxCounter) counter = 0;
            consoleSize = ConsoleHelper.GetConsoleSize(console);

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
