using System.Collections.Generic;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Native.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents the special field at MSH-2, which contains encoding characters for a message.
    /// </summary>
    internal sealed class NativeEncodingField : NativeElement, INativeField
    {
        public NativeEncodingField(NativeElement ancestor)
            : base(ancestor, 1, 2)
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
            get { return Ancestor.DescendantDivider[1]; }
            set
            {
                var s = Ancestor.DescendantDivider.Value;
                var builder = new StringBuilder();
                builder.Append(s.Substring(0, 3));
                builder.Append(value);
                builder.Append(s.Substring(4));
                Ancestor.DescendantDivider.Value = builder.ToString();
            }
        }

        public override INativeElement CloneDetached()
        {
            return new NativeField(Value, EncodingConfiguration);
        }

        public override INativeElement GetDescendant(int index)
        {
            return new NativeEncodingField(this);
        }

        protected override IStringDivider GetDescendantDivider(NativeElement ancestor, int index)
        {
            return new ProxyStringDivider(() => Ancestor.DescendantDivider[1], v => Ancestor.DescendantDivider[1] = v);
        }

        public override string ToString()
        {
            return Value;
        }

        public string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value;
        }

        public IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value.Yield();
        }

        public new INativeRepetition this[int index]
        {
            get { return new NativeRepetition(this, 0, 1); }
        }

        INativeField INativeField.CloneDetached()
        {
            return new NativeEncodingField(this);
        }
    }
}