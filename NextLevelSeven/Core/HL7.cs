namespace NextLevelSeven.Core
{
    // ReSharper disable once InconsistentNaming

    /// <summary>
    ///     Health Level Seven constants.
    /// </summary>
    static public class HL7
    {
        /// <summary>
        ///     String that represents a null but present value.
        /// </summary>
        public static readonly string Null = "\"\"";

        /// <summary>
        ///     Strings that will be considered as null.
        /// </summary>
        public static readonly string[] NullValues = { Null, null, string.Empty };
    }
}
