using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>Interface for a top-level message element builder.</summary>
    public interface IMessageBuilder : IMessage, IElementBuilder
    {
        /// <summary>Get a descendant segment builder.</summary>
        /// <param name="index">Index within the message to get the builder from.</param>
        /// <returns>Segment builder for the specified index.</returns>
        new ISegmentBuilder this[int index] { get; }

        /// <summary>Set a component's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetComponent(int segmentIndex, int fieldIndex, int repetition, int componentIndex, string value);

        /// <summary>Replace all component values within a field repetition.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetComponents(int segmentIndex, int fieldIndex, int repetition, params string[] components);

        /// <summary>Set a sequence of components within a field repetition, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetComponents(int segmentIndex, int fieldIndex, int repetition, int startIndex,
            params string[] components);

        /// <summary>Set a field's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetField(int segmentIndex, int fieldIndex, string value);

        /// <summary>Replace all field values within a segment.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetFields(int segmentIndex, params string[] fields);

        /// <summary>Set a sequence of fields within a segment, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="startIndex">Field index to begin replacing at.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetFields(int segmentIndex, int startIndex, params string[] fields);

        /// <summary>Set a field repetition's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetFieldRepetition(int segmentIndex, int fieldIndex, int repetition, string value);

        /// <summary>Replace all field repetitions within a field.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetFieldRepetitions(int segmentIndex, int fieldIndex, params string[] repetitions);

        /// <summary>Set a sequence of field repetitions within a field, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetFieldRepetitions(int segmentIndex, int fieldIndex, int startIndex,
            params string[] repetitions);

        /// <summary>Set this message's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetMessage(string value);

        /// <summary>Set a segment's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetSegment(int segmentIndex, string value);

        /// <summary>Replace all segments within this message.</summary>
        /// <param name="segments">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetSegments(params string[] segments);

        /// <summary>Set a sequence of segments within this message, beginning at the specified start index.</summary>
        /// <param name="startIndex">Segment index to begin replacing at.</param>
        /// <param name="segments">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetSegments(int startIndex, params string[] segments);

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetSubcomponent(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            int subcomponentIndex, string value);

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetSubcomponents(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents);

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This IMessageBuilder, for chaining purposes.</returns>
        IMessageBuilder SetSubcomponents(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            int startIndex, params string[] subcomponents);
    }
}