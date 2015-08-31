using System;
using System.Collections.Generic;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    /// Common interface for string dividers.
    /// </summary>
    internal interface IStringDivider : IEnumerable<string>
    {
        event EventHandler ValueChanged;

        string this[int index] { get; set; }
        char[] BaseValue { get; }
        int Count { get; }
        char Delimiter { get; }
        IStringDivider Divide(int index, char delimiter);
        IReadOnlyList<StringDivision> Divisions { get; }
        StringDivision GetSubDivision(int index);
        int Index { get; set; }
        string Value { get; set; }
        int Version { get; }
    }
}
