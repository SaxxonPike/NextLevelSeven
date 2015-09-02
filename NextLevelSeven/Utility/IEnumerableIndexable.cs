using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    public interface IEnumerableIndexable<TIndex, TItem> : IEnumerable<TItem>, IIndexable<TIndex, TItem>
    {
    }
}
