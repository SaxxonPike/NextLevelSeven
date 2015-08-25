using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors.Dividers
{
    sealed internal class StringDivision
    {
        public StringDivision(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public readonly int Offset;
        public readonly int Length;
    }
}
