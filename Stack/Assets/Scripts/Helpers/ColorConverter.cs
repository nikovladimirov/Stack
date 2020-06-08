using System.Text.RegularExpressions;
using UnityEngine;

namespace Helpers
{
    public class ColorConverter
    {
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color32 Transparent = new Color32(255, 255, 255, 255);

        private const string pattern = @"^([\da-fA-F]{2})([\da-fA-F]{2})([\da-fA-F]{2})([\da-fA-F]{2})?";
        public static Color32? HexToColor(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return null;

            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF

            if (hex.Length != 6 && hex.Length != 8)
                return null;

            var match = Regex.Match(hex, pattern);
            if (!match.Success)
                return null;

            var r = byte.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(match.Groups[3].Value, System.Globalization.NumberStyles.HexNumber);
            var a = (match.Groups.Count == 5 && !string.IsNullOrEmpty(match.Groups[4].Value))
                ? byte.Parse(match.Groups[4].Value, System.Globalization.NumberStyles.HexNumber)
                : (byte)255;

            return new Color32(r, g, b, a);
        }
    }
}