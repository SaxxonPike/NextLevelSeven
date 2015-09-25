using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>An encoding configuration wrapper that redirects character properties from a MessageBuilder.</summary>
    internal sealed class BuilderEncodingConfiguration : EncodingConfiguration, IEncoding
    {
        /// <summary>Create an encoding configuration from a message or segment.</summary>
        /// <param name="builder">Message builder to pull the characters from.</param>
        public BuilderEncodingConfiguration(Builder builder)
        {
            Builder = builder;
        }

        /// <summary>Builder to pull characters from.</summary>
        private Builder Builder { get; set; }

        char IEncoding.ComponentDelimiter
        {
            get { return Builder.ComponentDelimiter; }
            set { Builder.ComponentDelimiter = value; }
        }

        char IEncoding.EscapeCharacter
        {
            get { return Builder.EscapeCharacter; }
            set { Builder.EscapeCharacter = value; }
        }

        char IEncoding.FieldDelimiter
        {
            get { return Builder.FieldDelimiter; }
            set { Builder.FieldDelimiter = value; }
        }

        char IEncoding.RepetitionDelimiter
        {
            get { return Builder.RepetitionDelimiter; }
            set { Builder.RepetitionDelimiter = value; }
        }

        char IEncoding.SubcomponentDelimiter
        {
            get { return Builder.SubcomponentDelimiter; }
            set { Builder.SubcomponentDelimiter = value; }
        }

        public override char ComponentDelimiter
        {
            get { return Builder.ComponentDelimiter; }
            protected set { Builder.ComponentDelimiter = value; }
        }

        public override char EscapeCharacter
        {
            get { return Builder.EscapeCharacter; }
            protected set { Builder.EscapeCharacter = value; }
        }

        public override char FieldDelimiter
        {
            get { return Builder.FieldDelimiter; }
            protected set { Builder.FieldDelimiter = value; }
        }

        public override char RepetitionDelimiter
        {
            get { return Builder.RepetitionDelimiter; }
            protected set { Builder.RepetitionDelimiter = value; }
        }

        public override char SubcomponentDelimiter
        {
            get { return Builder.SubcomponentDelimiter; }
            protected set { Builder.SubcomponentDelimiter = value; }
        }
    }
}