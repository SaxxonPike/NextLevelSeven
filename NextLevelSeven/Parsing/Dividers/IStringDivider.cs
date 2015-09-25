using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>Common interface for string dividers.</summary>
    internal interface IStringDivider
    {
        string this[int index]
        {
            get;
            set;
        }

        char[] BaseValue
        {
            get;
        }

        int Count
        {
            get;
        }

        char Delimiter
        {
            get;
        }

        IReadOnlyList<StringDivision> Divisions
        {
            get;
        }

        int Index
        {
            get;
            set;
        }

        bool IsNull
        {
            get;
        }

        string Value
        {
            get;
            set;
        }

        IEnumerable<string> Values
        {
            get;
            set;
        }

        char[] ValueChars
        {
            get;
            set;
        }

        int Version
        {
            get;
        }

        event EventHandler ValueChanged;
        IStringDivider Divide(int index, char delimiter);
        StringDivision GetSubDivision(int index);
    }
}