using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>Interface for a subcomponent element builder.</summary>
    public interface ISubcomponentBuilder : IElementBuilder, ISubcomponent
    {
        /// <summary>Get the ancestor builder. Null if the element is an orphan.</summary>
        new IComponentBuilder Ancestor { get; }

        /// <summary>Set this subcomponent's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        ISubcomponentBuilder SetSubcomponent(string value);
    }
}