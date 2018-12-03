using System;

namespace NextLevelSeven.Core.Encoding
{
    /// <summary>
    /// Map between MSH-18 character encoding definitions and Windows code pages.
    /// </summary>
    internal static class Msh18EncodingMap
    {
        public static System.Text.Encoding GetEncoding(string name)
        {
            switch (name?.ToLowerInvariant())
            {
                case "iso ir14":
                case "iso ir87":
                case "iso ir159":
                    return System.Text.Encoding.GetEncoding(20932);
                case "8859/1":
                    return System.Text.Encoding.GetEncoding(28591);
                case "8859/2":
                    return System.Text.Encoding.GetEncoding(28592);
                case "8859/3":
                    return System.Text.Encoding.GetEncoding(28593);
                case "8859/4":
                    return System.Text.Encoding.GetEncoding(28594);
                case "8859/5":
                    return System.Text.Encoding.GetEncoding(28595);
                case "8859/6":
                    return System.Text.Encoding.GetEncoding(28596);
                case "8859/7":
                    return System.Text.Encoding.GetEncoding(28597);
                case "8859/8":
                    return System.Text.Encoding.GetEncoding(28598);
                case "8859/9":
                    return System.Text.Encoding.GetEncoding(28599);
                case "8859/15":
                    return System.Text.Encoding.GetEncoding(28605);
                default:
                    return System.Text.Encoding.UTF8;
            }
        }
    }
}