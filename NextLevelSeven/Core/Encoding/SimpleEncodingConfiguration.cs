namespace NextLevelSeven.Core.Encoding
{
    internal sealed class SimpleEncodingConfiguration : EncodingConfiguration
    {
        /// <summary>Create an encoding configuration with the default characters.</summary>
        public SimpleEncodingConfiguration()
        {
        }

        /// <summary>Clone an existing encoding configuration.</summary>
        /// <param name="other">Source configuration to pull values from.</param>
        public SimpleEncodingConfiguration(EncodingConfiguration other)
        {
            CopyFrom(other);
        }

        /// <summary>Create an encoding configuration with the specified characters.</summary>
        /// <param name="field">Field delimiter.</param>
        /// <param name="repetition">Repetition delimiter.</param>
        /// <param name="component">Component delimiter.</param>
        /// <param name="subcomponent">Subcomponent delimiter.</param>
        /// <param name="escape">Escape character.</param>
        public SimpleEncodingConfiguration(char field, char repetition, char component, char subcomponent, char escape)
        {
            InitializeWith(field, repetition, component, subcomponent, escape);
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
    }
}