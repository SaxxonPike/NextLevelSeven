namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Provides methods for escaping and unescaping HL7 strings.
    /// </summary>
    internal static class Escaping
    {
        /// <summary>
        ///     Escape a string so it can be represented in raw HL7 form.
        /// </summary>
        /// <param name="config">Encoding character configuration.</param>
        /// <param name="data">Data to escape.</param>
        /// <returns>Escaped string.</returns>
        public static string Escape(EncodingConfiguration config, string data)
        {
            return data;
        }

        /// <summary>
        ///     Unscape a string from raw HL7 form.
        /// </summary>
        /// <param name="config">Encoding character configuration.</param>
        /// <param name="data">Data to remove escaping from.</param>
        /// <returns>Unescaped string.</returns>
        public static string UnEscape(EncodingConfiguration config, string data)
        {
            return data;
        }
    }
}