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

        // 500 lines should be more than enough
        private List<string> cache = new List<string>(500);


        public IEnumerator<dynamic> Start()
        {
            yield return null;

        }

        int counter = 0;
        int maxCounter = 1024;
        void addLog()
        {

        }
        public void Update()
        {
            uiScale = console.canvas.scaleFactor * 3 / 4;
            counter++;
            if (counter > maxCounter) counter = 0;
            consoleSize = ConsoleHelper.GetConsoleSize(console, uiScale);
            if (cache.Count >= 500)
            {
                cache.RemoveAt(0);
            }
            cache.Add(counter.ToString());

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