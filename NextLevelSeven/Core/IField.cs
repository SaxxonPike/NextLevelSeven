using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>Represents a field element in an HL7 message.</summary>
    public interface IField : IElement
    {
        /// <summary>Get the ancestor segment. Null if the element is an orphan.</summary>
        new ISegment Ancestor { get; }

        /// <summary>Get this element's repetitions.</summary>
        IEnumerable<IRepetition> Repetitions { get; }

        /// <summary>Get a copy of the segment.</summary>
        new IField Clone();

        /// <summary>Get data from a specific place in the field. Depth is determined by how many indices are specified.</summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The the specified element.</returns>
        string GetValue(int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>Get data from a specific place in the field. Depth is determined by how many indices are specified.</summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1);
    }
}