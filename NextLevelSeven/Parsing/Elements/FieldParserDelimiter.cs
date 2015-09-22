namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents the special MSH-1 field, which contains the field delimiter for the rest of the segment.
    /// </summary>
    internal sealed class FieldParserDelimiter : FieldParserWithStaticValue
    {
        /// <summary>
        ///     Create a field delimiter descendant.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        public FieldParserDelimiter(ParserBase ancestor)
            : base(ancestor, 0, 1)
        {
        }

        /// <summary>
        ///     Get or set the value of the field delimiter.
        /// </summary>
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
    }
}