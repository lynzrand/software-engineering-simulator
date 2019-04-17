using UnityEngine;
using UnityEngine.UI;

namespace Sesim.Helpers.UI
{
    public struct ConsoleSize
    {
        public int width;
        public int height;
        public ConsoleSize(int height, int width)
        {
            this.width = width;
            this.height = height;
        }
    }
    public static class ConsoleHelper
    {
        public static ConsoleSize CalculateConsoleSize(Text text)
        {
            text.font.GetCharacterInfo(' ', out CharacterInfo info, text.fontSize, text.fontStyle);
            int fontWidth = info.advance;
            float fontHeight = text.lineSpacing * text.fontSize;
            float boundingBoxWidth = text.preferredWidth;
            float boundingBoxHeight = text.preferredHeight;
            int consoleWidth = Mathf.FloorToInt(boundingBoxWidth / fontWidth);
            int consoleHeight = Mathf.FloorToInt(boundingBoxHeight / fontHeight);
            return new ConsoleSize(consoleHeight, consoleWidth);
        }
    }
}