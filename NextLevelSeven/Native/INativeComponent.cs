using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Native
{
    public interface INativeComponent : INativeElement, IComponent
    {
        /// <summary>
        ///     Get a descendant subcomponent at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new INativeSubcomponent this[int index] { get; }
    }
}
