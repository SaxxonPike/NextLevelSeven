using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Interface for a subcomponent element builder.
    /// </summary>
    public interface ISubcomponentBuilder : IBuilder, ISubcomponent
    {
        /// <summary>
        ///     Set this subcomponent's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        ISubcomponentBuilder Subcomponent(string value);
    }
}