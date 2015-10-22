using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Parsing
{
    public interface IComponentParser : IElementParser, IComponent
    {
        /// <summary>Get the ancestor repetition. Null if the element is an orphan.</summary>
        new IRepetitionParser Ancestor { get; }

        /// <summary>
        ///     Get a descendant subcomponent at the specified index. Indices match the HL7 specification, and are not
        ///     necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new ISubcomponentParser this[int index] { get; }

        /// <summary>Get all subcomponents.</summary>
        new IEnumerable<ISubcomponentParser> Subcomponents { get; }
    }
}