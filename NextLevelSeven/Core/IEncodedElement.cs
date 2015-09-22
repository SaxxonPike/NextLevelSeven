using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Core
{
    internal interface IEncodedElement : IElement
    {
        EncodingConfigurationBase EncodingConfiguration { get; }
    }
}
