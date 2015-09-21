using NextLevelSeven.Parsing.Dividers;
using NextLevelSeven.Parsing.Elements;

namespace NextLevelSeven.Parsing
{
    internal interface IDividable
    {
        ElementParser Ancestor { get; }
        char Delimiter { get; }
        IStringDivider DescendantDivider { get; }
    }
}