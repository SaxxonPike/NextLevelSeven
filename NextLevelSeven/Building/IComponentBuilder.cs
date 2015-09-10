using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Building
{
    public interface IComponentBuilder : IBuilder
    {
        /// <summary>
        ///     Get a descendant subcomponent builder.
        /// </summary>
        /// <param name="index">Index within the component to get the builder from.</param>
        /// <returns>Subcomponent builder for the specified index.</returns>
        ISubcomponentBuilder this[int index] { get; }

        /// <summary>
        ///     Set this component's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This IComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder Component(string value);

        /// <summary>
        ///     Set a subcomponent's content.
        /// </summary>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This IComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder Subcomponent(int subcomponentIndex, string value);

        /// <summary>
        ///     Replace all subcomponents within this component.
        /// </summary>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder Subcomponents(params string[] subcomponents);

        /// <summary>
        ///     Set a sequence of subcomponents within this component, beginning at the specified start index.
        /// </summary>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        IComponentBuilder Subcomponents(int startIndex, params string[] subcomponents);
    }
}
