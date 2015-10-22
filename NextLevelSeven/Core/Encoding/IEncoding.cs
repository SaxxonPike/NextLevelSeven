namespace NextLevelSeven.Core.Encoding
{
    /// <summary>Represents message-wide encoding characters.</summary>
    public interface IEncoding : IReadOnlyEncoding
    {
        /// <summary>Get or set the character used to separate component-level content.</summary>
        new char ComponentDelimiter { get; set; }

        /// <summary>Get or set the character used to signify escape sequences.</summary>
        new char EscapeCharacter { get; set; }

        /// <summary>Get or set the character used to separate fields.</summary>
        new char FieldDelimiter { get; set; }

        /// <summary>Get or set the character used to separate field repetition content.</summary>
        new char RepetitionDelimiter { get; set; }

        /// <summary>Get or set the character used to separate subcomponent-level content.</summary>
        new char SubcomponentDelimiter { get; set; }
    }
}