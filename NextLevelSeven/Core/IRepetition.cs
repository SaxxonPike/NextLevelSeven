using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>Represents a field repetition element in an HL7 message.</summary>
    public interface IRepetition : IElement
    {
        /// <summary>Get the ancestor field. Null if the element is an orphan.</summary>
        new IField Ancestor { get; }

        /// <summary>Get this element's components.</summary>
        IEnumerable<IComponent> Components { get; }

        /// <summary>Get a copy of the segment.</summary>
        new IRepetition Clone();

        /// <summary>Get data from a specific place in the field repetition. Depth is determined by how many indices are specified.</summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The the specified element.</returns>
        string GetValue(int component = -1, int subcomponent = -1);

        /// <summary>Get data from a specific place in the field repetition. Depth is determined by how many indices are specified.</summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues(int component = -1, int subcomponent = -1);
    }
}