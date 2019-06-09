using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using CommandLine;
using System.Text.RegularExpressions;

namespace Sesim.Game.Controllers.MainGame
{
    public class StatusConsoleController : MonoBehaviour
    {
        public CompanyActionController src;
        public InputField consoleField;

        void Start()
        {
            consoleField.onEndEdit.AddListener(onConsoleEditEnded);
        }

        void onConsoleEditEnded(string value)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                consoleField.text = "";
                ResolveAction(value);
            }
        }

        void ResolveAction(string value)
        {
            var cmdLine = ParseArgumentsWindows(value);
            var result = Parser.Default.ParseArguments<DebugOptions>(cmdLine)
            .WithParsed(options =>
            {
                Debug.Log(options);

            }).WithNotParsed(errs =>
            {
                foreach (var err in errs) Debug.Log(err);
            });
        }

        void Update()
        {
            // consoleField.
        }

        #region commandline

        internal class DebugOptions
        {
            [Option('V')]
            public bool Verbose { get; set; }
        }

        // https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp
        private static readonly Regex RxWinArgs
            = new Regex("([^\\s\"]+\"|((?<=\\s|^)(?!\"\"(?!\"))\")+)(\"\"|.*?)*\"[^\\s\"]*|[^\\s]+",
                RegexOptions.Compiled
                | RegexOptions.Singleline
                | RegexOptions.ExplicitCapture
                | RegexOptions.CultureInvariant);

        internal static IEnumerable<string> ParseArgumentsWindows(string args)
        {
            var match = RxWinArgs.Match(args);

            while (match.Success)
            {
                yield return match.Value;
                match = match.NextMatch();
            }
        }

        #endregion
    }
}
