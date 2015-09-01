using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    /// Contains information about a delimited piece of a string.
    /// </summary>
    internal struct StringDivision
    {
        /// <summary>
        /// Get a division marked invalid.
        /// </summary>
        public static readonly StringDivision Invalid = new StringDivision();

        /// <summary>
        /// Create a new division.
        /// </summary>
        /// <param name="offset">Offset of the division.</param>
        /// <param name="length">Length of the division.</param>
        public StringDivision(int offset, int length)
        {
            Offset = offset;
            Length = length;
            Valid = true;
        }

        /// <summary>
        /// Index the value starts.
        /// </summary>
        public readonly int Offset;
        
        /// <summary>
        /// Length of the value.
        /// </summary>
        public readonly int Length;

        /// <summary>
        /// Returns true if this is a valid division.
        /// </summary>
        public readonly bool Valid;
    }
}
