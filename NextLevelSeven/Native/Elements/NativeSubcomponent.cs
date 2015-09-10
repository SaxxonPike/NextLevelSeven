using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a subcomponent-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeSubcomponent : NativeElement
    {
        private readonly EncodingConfiguration _encodingConfigurationOverride;

        public NativeSubcomponent(NativeElement ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        private NativeSubcomponent(string value, EncodingConfiguration config)
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
            get { return 0; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override bool HasSignificantDescendants
        {
            get { return false; }
        }

        public override INativeElement CloneDetached()
        {
            return new NativeSubcomponent(Value, EncodingConfiguration);
        }

        public override INativeElement GetDescendant(int index)
        {
            throw new ElementException(ErrorCode.SubcomponentCannotHaveDescendants);
        }
    }
}