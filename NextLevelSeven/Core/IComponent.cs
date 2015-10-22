using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>Represents a component element in an HL7 message.</summary>
    public interface IComponent : IElement
    {
        /// <summary>Get the ancestor repetition. Null if the element is an orphan.</summary>
        new IRepetition Ancestor { get; }

        /// <summary>Get this element's subcomponents.</summary>
        IEnumerable<ISubcomponent> Subcomponents { get; }

        /// <summary>Get a copy of the segment.</summary>
        new IComponent Clone();

        /// <summary>Get data from a specific place in the message. Depth is determined by how many indices are specified.</summary>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The the specified element.</returns>
        string GetValue(int subcomponent = -1);

        /// <summary>Get data from a specific place in the field repetition. Depth is determined by how many indices are specified.</summary>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues(int subcomponent = -1);
    }
}