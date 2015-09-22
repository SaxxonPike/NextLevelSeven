using System.Text;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents the special field at MSH-2, which contains encoding characters for a message.
    /// </summary>
    internal sealed class EncodingFieldParser : FieldParserWithStaticValue
    {
        /// <summary>
        ///     Create an encoding field.
        /// </summary>
        /// <param name="ancestor"></param>
        public EncodingFieldParser(ParserBase ancestor)
            : base(ancestor, 1, 2)
        {
        }

        /// <summary>
        ///     Get or set encoding characters.
        /// </summary>
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
    }
}