using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Native.Dividers;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Native
{
    internal interface IDividable
    {
        NativeElement Ancestor { get; }
        char Delimiter { get; }
        IStringDivider DescendantDivider { get; }
    }
}
