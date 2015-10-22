namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>Contains information about a delimited piece of a string.</summary>
    internal struct StringDivision
    {
        /// <summary>Get a division marked invalid.</summary>
        public static readonly StringDivision Invalid = new StringDivision();

        /// <summary>Length of the value.</summary>
        public readonly int Length;

        /// <summary>Index the value starts.</summary>
        public readonly int Offset;

        /// <summary>Returns true if this is a valid division.</summary>
        public readonly bool Valid;

        /// <summary>Create a new division.</summary>
        /// <param name="offset">Offset of the division.</param>
        /// <param name="length">Length of the division.</param>
        public StringDivision(int offset, int length)
        {
            Offset = offset;
            Length = length;
            Valid = true;
        }
    }
}