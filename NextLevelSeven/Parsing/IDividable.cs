using NextLevelSeven.Native.Dividers;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Native
{
    internal interface IDividable
    {
        ElementParser Ancestor { get; }
        char Delimiter { get; }
        IStringDivider DescendantDivider { get; }
    }
}