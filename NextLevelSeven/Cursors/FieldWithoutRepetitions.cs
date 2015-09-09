using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Represents a field that does not use a repetition delimiter (repeats are considered part of the value.)
    /// </summary>
    internal sealed class FieldWithoutRepetitions : Element
    {
        private readonly EncodingConfiguration _encodingConfigurationOverride;

        public FieldWithoutRepetitions(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private FieldWithoutRepetitions(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override int DescendantCount
        {
            get { return 1; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override INativeElement CloneDetached()
        {
            return new FieldWithoutRepetitions(Value, EncodingConfiguration);
        }

        public override INativeElement GetDescendant(int index)
        {
            return new Repetition(this, index - 1, index);
        }
    }
}