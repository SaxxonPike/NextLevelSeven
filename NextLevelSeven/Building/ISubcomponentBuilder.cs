namespace NextLevelSeven.Building
{
    public interface ISubcomponentBuilder : IBuilder
    {
        /// <summary>
        ///     Set this subcomponent's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        ISubcomponentBuilder Subcomponent(string value);
    }
}