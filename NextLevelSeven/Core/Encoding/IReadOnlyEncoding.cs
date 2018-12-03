namespace NextLevelSeven.Core.Encoding
{
    /// <summary>Represents message-wide encoding characters.</summary>
    public interface IReadOnlyEncoding
    {
        /// <summary>Get the character used to separate component-level content.</summary>
        char ComponentDelimiter { get; }

        /// <summary>Get the character used to signify escape sequences.</summary>
        char EscapeCharacter { get; }

        /// <summary>Get the character used to separate fields.</summary>
        char FieldDelimiter { get; }

        /// <summary>Get the character used to separate field repetition content.</summary>
        char RepetitionDelimiter { get; }

        /// <summary>Get the character used to separate subcomponent-level content.</summary>
        char SubcomponentDelimiter { get; }
        
        /// <summary>Get the current character encoding.</summary>
        System.Text.Encoding CharacterEncoding { get; }
    }
}