using NextLevelSeven.Core;

namespace NextLevelSeven.Building
{
    public sealed class SubcomponentBuilder : BuilderBase
    {
        /// <summary>
        ///     Internal subcomponent value.
        /// </summary>
        private string _value;

        /// <summary>
        ///     Create a subcomponent builder.
        /// </summary>
        internal SubcomponentBuilder(EncodingConfiguration encodingConfiguration)
            : base(encodingConfiguration)
        {
        }

        /// <summary>
        ///     Set this subcomponent's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        public SubcomponentBuilder Subcomponent(string value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        ///     Copy the contents of this builder to a string.
        /// </summary>
        /// <returns>Converted subcomponent.</returns>
        public override string ToString()
        {
            return _value ?? string.Empty;
        }
    }
}