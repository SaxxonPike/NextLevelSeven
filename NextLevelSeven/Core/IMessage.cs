using System;
using System.Collections.Generic;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// An interface to get values at any level in the HL7 message heirarchy.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        string GetValue(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>
        ///     Get first available data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segmentName">Segment name.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        string GetValue(string segmentName, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1);

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segmentName">Segment name.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        IEnumerable<string> GetValues(string segmentName, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1);
    }
}
