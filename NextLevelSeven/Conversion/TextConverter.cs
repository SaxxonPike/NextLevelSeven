using System.Collections.Generic;

namespace NextLevelSeven.Conversion
{
    static public class TextConverter
    {
        /// <summary>
        ///     Convert from standard string to HL7 formatted text.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <param name="delimiter">Delimiter to use (typically component)</param>
        /// <returns>Converted string.</returns>
        public static string ConvertFromFormattedText(IEnumerable<string> input, char delimiter)
        {
            return string.Join(new string(delimiter, 1), input);
        }

        /// <summary>
        ///     Convert from standard string to HL7 string.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <returns>Converted string.</returns>
        public static string ConvertFromString(string input)
        {
            return input;
        }

        /// <summary>
        ///     Convert from standard string to HL7 formatted text.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <param name="delimiter">Delimiter to use (typically component)</param>
        /// <returns>Converted string.</returns>
        public static IEnumerable<string> ConvertToFormattedText(string input, char delimiter)
        {
            return input.Split(delimiter);
        }

        /// <summary>
        ///     Convert HL7 string to standard string.
        /// </summary>
        /// <param name="input">String to convert.</param>
        /// <returns>Converted string.</returns>
        public static string ConvertToString(string input)
        {
            return input;
        }
    }
}
