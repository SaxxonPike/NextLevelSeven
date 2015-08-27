using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors.Dividers
{
    internal interface IStringDivider : IEnumerable<string>
    {
        string this[int index] { get; set; }
        string BaseValue { get; }
        int Count { get; }
        void Delete(int index);
        char Delimiter { get; }
        IStringDivider Divide(int index, char delimiter);
        IReadOnlyList<StringDivision> Divisions { get; }
        StringDivision GetSubDivision(int index);
        int Index { get; set; }
        string Value { get; set; }
        int Version { get; }
    }
}
