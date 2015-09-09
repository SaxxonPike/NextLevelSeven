using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    public interface IEnumerableIndexable<in TIndex, TItem> : IEnumerable<TItem>, IIndexable<TIndex, TItem>
    {
    }
}
