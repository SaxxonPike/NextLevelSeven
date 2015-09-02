using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    public interface IReadOnlyIndexable<TIndex, TItem> : IIndexable<TIndex, TItem>
    {
        new TItem this[TIndex index] { get; }
    }
}
