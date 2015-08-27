using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Transformation
{
    /// <summary>
    /// Interface for a factory to generate element transforms.
    /// </summary>
    public interface IElementTransformFactory
    {
        ElementTransform CreateTransform(IElement element);
    }
}
