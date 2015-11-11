using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>Interface for a field element builder.</summary>
    public interface IFieldBuilder : IElementBuilder, IField
    {
        /// <summary>Get the ancestor builder. Null if the element is an orphan.</summary>
        new ISegmentBuilder Ancestor { get; }

        /// <summary>Get a descendant field repetition builder.</summary>
        /// <param name="index">Index within the field to get the builder from.</param>
        /// <returns>Field repetition builder for the specified index.</returns>
        new IRepetitionBuilder this[int index] { get; }

        /// <summary>Set a component's content.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetComponent(int repetition, int componentIndex, string value);

        /// <summary>Replace all component values within a field repetition.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetComponents(int repetition, params string[] components);

        /// <summary>Set a sequence of components within a field repetition, beginning at the specified start index.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetComponents(int repetition, int startIndex, params string[] components);

        /// <summary>Set this field's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetField(string value);

        /// <summary>Set a field repetition's content.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetFieldRepetition(int repetition, string value);

        /// <summary>Replace all field repetitions within this field.</summary>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetFieldRepetitions(params string[] repetitions);

        /// <summary>Set a sequence of field repetitions within this field, beginning at the specified start index.</summary>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetFieldRepetitions(int startIndex, params string[] repetitions);

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetSubcomponent(int repetition, int componentIndex, int subcomponentIndex, string value);

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetSubcomponents(int repetition, int componentIndex, params string[] subcomponents);

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This IFieldBuilder, for chaining purposes.</returns>
        IFieldBuilder SetSubcomponents(int repetition, int componentIndex, int startIndex, params string[] subcomponents);
    }
}