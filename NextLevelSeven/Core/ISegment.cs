using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    public interface ISegment : IElement
    {
        string Type { get; set; }
    }
}
