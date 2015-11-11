using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>Interface for a component element builder.</summary>
    public interface IComponentBuilder : IElementBuilder, IComponent
    {
        /// <summary>Get the ancestor builder. Null if the element is an orphan.</summary>
        new IRepetitionBuilder Ancestor { get; }

        /// <summary>Get a descendant subcomponent builder.</summary>
        /// <param name="index">Index within the component to get the builder from.</param>
        /// <returns>Subcomponent builder for the specified index.</returns>
        new ISubcomponentBuilder this[int index] { get; }

        /// <summary>Set this component's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This IComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder SetComponent(string value);

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder SetSubcomponent(int subcomponentIndex, string value);

        /// <summary>Replace all subcomponents within this component.</summary>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder SetSubcomponents(params string[] subcomponents);

        /// <summary>Set a sequence of subcomponents within this component, beginning at the specified start index.</summary>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder SetSubcomponents(int startIndex, params string[] subcomponents);
    }
}