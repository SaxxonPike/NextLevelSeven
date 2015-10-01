using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>Interface for a segment element builder.</summary>
    public interface ISegmentBuilder : IElementBuilder, ISegment
    {
        /// <summary>Get the ancestor builder. Null if the element is an orphan.</summary>
        new IMessageBuilder Ancestor { get; }

        /// <summary>Get a descendant field builder.</summary>
        /// <param name="index">Index within the segment to get the builder from.</param>
        /// <returns>Field builder for the specified index.</returns>
        new IFieldBuilder this[int index] { get; }

        /// <summary>Set a component's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetComponent(int fieldIndex, int repetition, int componentIndex, string value);

        /// <summary>Replace all component values within a field repetition.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetComponents(int fieldIndex, int repetition, params string[] components);

        /// <summary>Set a sequence of components within a field repetition, beginning at the specified start index.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetComponents(int fieldIndex, int repetition, int startIndex, params string[] components);

        /// <summary>Set a field's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetField(int fieldIndex, string value);

        /// <summary>Replace all field values within this segment.</summary>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetFields(params string[] fields);

        /// <summary>Set a sequence of fields within this segment, beginning at the specified start index.</summary>
        /// <param name="startIndex">Field index to begin replacing at.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetFields(int startIndex, params string[] fields);

        /// <summary>Set a field repetition's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetFieldRepetition(int fieldIndex, int repetition, string value);

        /// <summary>Replace all field repetitions within a field.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetFieldRepetitions(int fieldIndex, params string[] repetitions);

        /// <summary>Set a sequence of field repetitions within a field, beginning at the specified start index.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetFieldRepetitions(int fieldIndex, int startIndex, params string[] repetitions);

        /// <summary>Set this segment's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetSegment(string value);

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetSubcomponent(int fieldIndex, int repetition, int componentIndex, int subcomponentIndex,
            string value);

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetSubcomponents(int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents);

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This ISegmentBuilder, for chaining purposes.</returns>
        ISegmentBuilder SetSubcomponents(int fieldIndex, int repetition, int componentIndex, int startIndex,
            params string[] subcomponents);
    }
}