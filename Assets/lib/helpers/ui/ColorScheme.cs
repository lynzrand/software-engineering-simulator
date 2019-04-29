using System;
using System.Globalization;
using UnityEngine;

namespace Sesim.Helpers.UI
{
    /// <summary>
    /// <p> This class defines a color scheme based on base16's style guide.
    /// <p> Check https://github.com/chriskempson/base16/blob/master/styling.md 
    /// for more information
    /// </summary>
    public class Base16ColorScheme
    {
        /// <summary>
        /// Default background
        /// </summary>
        public Color32 base00;

        /// <summary>
        /// Lighter Background
        /// </summary>
        public Color32 base01;

        /// <summary>
        /// Selection Background
        /// </summary>
        public Color32 base02;

        /// <summary>
        /// Highlighted Background; Comments
        /// </summary>
        public Color32 base03;

        /// <summary>
        /// Dark foreground
        /// </summary>
        public Color32 base04;

        /// <summary>
        /// Default foreground
        /// </summary>
        public Color32 base05;

        /// <summary>
        /// Light foreground
        /// </summary>
        public Color32 base06;

        /// <summary>
        /// Light background
        /// </summary>
        public Color32 base07;

        /// <summary>
        /// (RED) Variables, XML Tags, Markup Link Text, Markup Lists, Diff Deleted
        /// </summary>
        public Color32 base08;

        /// <summary>
        /// (ORANGE) Integers, Boolean, Constants, XML Attributes, Markup Link Url
        /// </summary>
        public Color32 base09;

        /// <summary>
        /// (YELLOW) Classes, Markup Bold, Search Text Background
        /// </summary>
        public Color32 base0A;

        /// <summary>
        /// (GREEN) Strings, Inherited Class, Markup Code, Diff Inserted
        /// </summary>
        public Color32 base0B;

        /// <summary>
        /// (CYAN) Support, Regular Expressions, Escape Characters, Markup Quotes
        /// </summary>
        public Color32 base0C;

        /// <summary>
        /// (BLUE) Functions, Methods, Attribute IDs, Headings
        /// </summary>
        public Color32 base0D;

        /// <summary>
        /// (PURPLE) Keywords, Storage, Selector, Markup Italic, Diff Changed
        /// </summary>
        public Color32 base0E;

        /// <summary>
        /// (PINK) Deprecated, Opening/Closing Embedded Language Tags,
        /// </summary>
        public Color32 base0F;

        public Base16ColorScheme(
            string base00, string base01, string base02, string base03,
            string base04, string base05, string base06, string base07,
            string base08, string base09, string base0A, string base0B,
            string base0C, string base0D, string base0E, string base0F)
        {
            this.base00 = stringToColor(base00);
            this.base01 = stringToColor(base01);
            this.base02 = stringToColor(base02);
            this.base03 = stringToColor(base03);
            this.base04 = stringToColor(base04);
            this.base05 = stringToColor(base05);
            this.base06 = stringToColor(base06);
            this.base07 = stringToColor(base07);
            this.base08 = stringToColor(base08);
            this.base09 = stringToColor(base09);
            this.base0A = stringToColor(base0A);
            this.base0B = stringToColor(base0B);
            this.base0C = stringToColor(base0C);
            this.base0D = stringToColor(base0D);
            this.base0E = stringToColor(base0E);
            this.base0F = stringToColor(base0F);
        }

        /// <summary>
        /// Parse color strings in ARGB or RGBA format into Color32
        /// </summary>
        /// <param name="color">The string representation</param>
        /// <returns></returns>
        public static Color32 stringToColor(string color)
        {
            Int32 rgb = Int32.Parse(color, NumberStyles.HexNumber);
            byte r, g, b, a;
            switch (color.Length)
            {
                case 3:
                    r = (byte)(((rgb & 0xf00) >> 8) * 0x11);
                    g = (byte)(((rgb & 0x0f0) >> 4) * 0x11);
                    b = (byte)((rgb & 0x00f) * 0x11);
                    a = 0xff;
                    break;
                case 4:
                    r = (byte)(((rgb & 0xf000) >> 12) * 0x11);
                    g = (byte)(((rgb & 0x0f00) >> 8) * 0x11);
                    b = (byte)(((rgb & 0x00f0) >> 4) * 0x11);
                    a = (byte)(((rgb & 0x000f)) * 0x11);
                    break;
                case 6:
                    r = (byte)((rgb & 0xff0000) >> 16);
                    g = (byte)((rgb & 0x00ff00) >> 8);
                    b = (byte)((rgb & 0x0000ff));
                    a = 0xff;
                    break;
                case 8:
                    r = (byte)((rgb & 0xff000000) >> 24);
                    g = (byte)((rgb & 0x00ff0000) >> 16);
                    b = (byte)((rgb & 0x0000ff00) >> 8);
                    a = (byte)((rgb & 0x000000ff));
                    break;
                default:
                    throw new ArgumentException($"{color} is not a valid RGB or ARGB color!");
            }
            return new Color32(r, g, b, a);
        }

        public static Base16ColorScheme MaterialPaleNight = new Base16ColorScheme(
            base00: "292D3E",
            base01: "32374D",
            base02: "444267",
            base03: "676E95",
            base04: "8796B0",
            base05: "959DCB",
            base06: "959DCB",
            base07: "FFFFFF",
            base08: "F07178",
            base09: "F78C6C",
            base0A: "FFCB6B",
            base0B: "C3E88D",
            base0C: "89DDFF",
            base0D: "82AAFF",
            base0E: "C792EA",
            base0F: "FF5370"
        );
    }
}