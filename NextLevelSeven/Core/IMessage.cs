using System.Collections.Generic;
using NextLevelSeven.Core.Properties;

namespace NextLevelSeven.Core
{
    /// <summary>Represents an HL7 message.</summary>
    public interface IMessage : IElement
    {
        /// <summary>Access message details as a property set.</summary>
        IMessageDetails Details { get; }

        /// <summary>Get this message's segments.</summary>
        IEnumerable<ISegment> Segments { get; }

        /// <summary>Get a copy of the message.</summary>
        new IMessage Clone();

        /// <summary>Get data from a specific place in the message. Depth is determined by how many indices are specified.</summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The the specified element.</returns>
        string GetValue(int segment = -1, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>Get data from a specific place in the message. Depth is determined by how many indices are specified.</summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1);
    }
}