using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    public interface IIndexable<TIndex, TItem>
    {
        TItem this[TIndex index] { get; set; }
    }
}
