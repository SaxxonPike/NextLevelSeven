using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>Represents a segment element in an HL7 message.</summary>
    public interface ISegment : IElement
    {
        /// <summary>Get the ancestor message. Null if the element is an orphan.</summary>
        new IMessage Ancestor { get; }

        /// <summary>Get or set the three-letter type code for the segment.</summary>
        string Type { get; set; }

        /// <summary>Get this element's fields.</summary>
        IEnumerable<IField> Fields { get; }

        /// <summary>Get a copy of the segment.</summary>
        new ISegment Clone();

        /// <summary>Get data from a specific place in the segment. Depth is determined by how many indices are specified.</summary>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The the specified element.</returns>
        string GetValue(int field = -1, int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>Get data from a specific place in the segment. Depth is determined by how many indices are specified.</summary>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues(int field = -1, int repetition = -1, int component = -1, int subcomponent = -1);
    }
}