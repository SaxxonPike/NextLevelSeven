using NextLevelSeven.Core;
using NextLevelSeven.Native.Dividers;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents the special MSH-1 field, which contains the field delimiter for the rest of the segment.
    /// </summary>
    internal sealed class NativeFieldDelimiter : NativeElement
    {
        public NativeFieldDelimiter(NativeElement ancestor)
            : base(ancestor, 0, 1)
        {
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override bool HasSignificantDescendants
        {
            get { return false; }
        }

        public override string Value
        {
            get
            {
                var value = Ancestor.DescendantDivider.Value;
                if (value != null && value.Length > 3)
                {
                    return new string(value[3], 1);
                }
                return null;
            }
            set
            {
                // TODO: change the other delimiters in the segment
                var s = Ancestor.DescendantDivider.Value;
                if (s != null && s.Length >= 3)
                {
                    Ancestor.DescendantDivider.Value = string.Join(s.Substring(0, 3), value,
                        (s.Length > 3 ? s.Substring(4) : string.Empty));
                }
            }
        }

        public override INativeElement CloneDetached()
        {
            return new NativeField(Value, EncodingConfiguration);
        }

        public override INativeElement GetDescendant(int index)
        {
            return new NativeFieldDelimiter(this);
        }

        protected override IStringDivider GetDescendantDivider(NativeElement ancestor, int index)
        {
            return new ProxyStringDivider(() => Value, v => Value = v);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}