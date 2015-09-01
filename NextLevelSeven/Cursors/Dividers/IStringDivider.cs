using System;
using System.Collections.Generic;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    ///     Common interface for string dividers.
    /// </summary>
    internal interface IStringDivider : IEnumerable<string>
    {
        string this[int index] { get; set; }
        char[] BaseValue { get; }
        int Count { get; }
        char Delimiter { get; }
        IReadOnlyList<StringDivision> Divisions { get; }
        int Index { get; set; }
        string Value { get; set; }
        char[] ValueChars { get; set; }
        int Version { get; }
        event EventHandler ValueChanged;
        void Delete(int index);
        void Delete(StringDivision division);
        IStringDivider Divide(int index, char delimiter);
        StringDivision GetSubDivision(int index);
    }
}