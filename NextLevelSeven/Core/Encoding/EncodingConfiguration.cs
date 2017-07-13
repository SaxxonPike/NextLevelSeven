namespace NextLevelSeven.Core.Encoding
{
    internal sealed class EncodingConfiguration : ReadOnlyEncodingConfiguration, IEncoding
    {
        /// <summary>Create an encoding configuration with the default characters.</summary>
        public EncodingConfiguration()
        {
        }

        /// <summary>Clone an existing encoding configuration.</summary>
        /// <param name="other">Source configuration to pull values from.</param>
        public EncodingConfiguration(IReadOnlyEncoding other)
        {
            CopyFrom(other);
        }

        /// <summary>Get the component delimiter.</summary>
        public override char ComponentDelimiter { get; protected set; }

        /// <summary>Get the escape character.</summary>
        public override char EscapeCharacter { get; protected set; }

        /// <summary>Get the field delimiter.</summary>
        public override char FieldDelimiter { get; protected set; }

        /// <summary>Get the repetition delimiter.</summary>
        public override char RepetitionDelimiter { get; protected set; }

        /// <summary>Get the subcomponent delimiter.</summary>
        public override char SubcomponentDelimiter { get; protected set; }

        char IEncoding.ComponentDelimiter
        {
            get => ComponentDelimiter;
            set => ComponentDelimiter = value;
        }

        char IEncoding.EscapeCharacter
        {
            get => EscapeCharacter;
            set => EscapeCharacter = value;
        }

        char IEncoding.FieldDelimiter
        {
            get => FieldDelimiter;
            set => FieldDelimiter = value;
        }

        char IEncoding.RepetitionDelimiter
        {
            get => RepetitionDelimiter;
            set => RepetitionDelimiter = value;
        }

        char IEncoding.SubcomponentDelimiter
        {
            get => SubcomponentDelimiter;
            set => SubcomponentDelimiter = value;
        }
    }
}