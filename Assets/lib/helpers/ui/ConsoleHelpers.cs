using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

namespace Sesim.Helpers.UI
{
    public static class ConsoleHelper
    {
        /// <summary>
        /// Given a text area, calculate the console size in characters.
        /// 
        /// **WARNING: this method only works with monospace fonts!**
        /// </summary>
        /// <param name="text">The textarea to be used</param>
        /// <returns>The console's size in x and y axes</returns>
        public static Vector2Int GetConsoleSize(Text text)
        {
            text.font.RequestCharactersInTexture(" ", text.fontSize, text.fontStyle);
            text.font.GetCharacterInfo(' ', out CharacterInfo info, text.fontSize, text.fontStyle);

            var scaleFactor = text.canvas.scaleFactor;

            // Unity requires fonts to be pixel perfect whether the "pixel perfect"
            // tick is ticked or not in the root canvas
            float fontWidth = Mathf.Round(info.advance * scaleFactor) / scaleFactor;
            float fontHeight = Mathf.Round(text.fontSize * text.lineSpacing * scaleFactor) / scaleFactor;

            var rect = text.GetComponent<RectTransform>().rect;

            float boundingBoxWidth = rect.xMax - rect.xMin;
            float boundingBoxHeight = rect.yMax - rect.yMin;

            int consoleWidth = Mathf.FloorToInt(boundingBoxWidth / fontWidth);
            int consoleHeight = Mathf.FloorToInt(boundingBoxHeight / fontHeight);
            if (consoleWidth < 0) consoleWidth = 0;
            if (consoleHeight < 0) consoleHeight = 0;
            return new Vector2Int(x: consoleWidth, y: consoleHeight);
        }

        /// <summary>
        /// Generate a progressbar that looks like like
        /// ```
        /// [========>             ]
        /// ```
        /// </summary>
        /// <param name="width">The width of the progress bar in chars</param>
        /// <param name="progress">The progress percent to display</param>
        /// <param name="startCap">The character to be displayed at the start, default `[`</param>
        /// <param name="endCap"> ... at the end, default `]`</param>
        /// <param name="filled">The character representing filled parts, default `=`</param>
        /// <param name="filledCap">The character place on the end of filled section</param>
        /// <param name="empty">The character representing empty parts, default ` `</param>
        /// <returns>The string progressbar</returns>
        public static string GenerateProgressBar(
            int width, float progress,
            char startCap = '[', char endCap = ']',
            char filled = '=', char empty = ' ', char filledCap = '>', bool useCap = true)
        {
            if (progress < 0) progress = 0; else if (progress > 1) progress = 1;
            StringBuilder sb = new StringBuilder();
            int progressBarSize = width - 2;
            int progressBarFilledSize = Mathf.RoundToInt(progressBarSize * progress);
            bool filledHasCap = (useCap && progress > 0 && progress < 1);
            if (filledHasCap && progressBarFilledSize > 0) progressBarFilledSize -= 1;
            sb.Append(startCap);
            sb.Append(filled, progressBarFilledSize);
            if (filledHasCap) sb.Append(filledCap);
            sb.Append(empty, progressBarSize - progressBarFilledSize - (filledHasCap ? 1 : 0));
            sb.Append(endCap);
            return sb.ToString();
        }


        /// <summary>
        /// Generates a fixed-width representation of action and description
        /// that looks like
        /// 
        /// ```
        /// Action                Destination
        /// ^~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ total: width characters
        /// ```
        /// </summary>
        /// <param name="width">The total width of the representation</param>
        /// <param name="action">The action on the left</param>
        /// <param name="destination">The destination on the right</param>
        /// <param name="maxActionFillPercent">The max amount of space action can take up</param>
        /// <param name="cutoffCharacter">
        /// The character to use if we need to cut off in the middle</param>
        /// <returns>The string representation</returns>
        public static string GenerateTrimmedActionDescription(
            int width, string action, string destination,
            float maxActionFillPercent = 0.8f, char cutoffCharacter = '…')
        {
            if (width <= 6)
                return new StringBuilder().Append(cutoffCharacter, width).ToString();

            StringBuilder sb = new StringBuilder();
            int actionMaxSize = Mathf.RoundToInt(width * maxActionFillPercent);
            if (action.Length > actionMaxSize)
                action = action.Substring(0, actionMaxSize - 1) + cutoffCharacter;

            int destinationMaxSize = width - action.Length - 1;
            if (destination.Length > destinationMaxSize - 1)
                destination = destination.Substring(0, destinationMaxSize - 2) + cutoffCharacter;

            int remainingSize = width - action.Length - destination.Length;
            sb.Append(action);
            sb.Append(' ', remainingSize);
            sb.Append(destination);
            return sb.ToString();
        }
    }

    public delegate int RunConsoleApp(string[] args);


}