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
    sealed internal class StringDivision
    {
        public StringDivision(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        /// <summary>
        /// Index the value starts.
        /// </summary>
        public readonly int Offset;
        
        /// <summary>
        /// Length of the value.
        /// </summary>
        public readonly int Length;
    }
}
