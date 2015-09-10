using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    internal sealed class SubcomponentBuilder : BuilderBaseDescendant, ISubcomponentBuilder
    {
        /// <summary>
        ///     Internal subcomponent value.
        /// </summary>
        private string _value;

        /// <summary>
        ///     Create a subcomponent builder.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        internal SubcomponentBuilder(BuilderBase builder)
            : base(builder)
        {
        }

        /// <summary>
        ///     Set this subcomponent's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        public ISubcomponentBuilder Subcomponent(string value)
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
            return Value;
        }

        /// <summary>
        /// Get or set the component string.
        /// </summary>
        public string Value
        {
            get { return _value ?? string.Empty; }
            set { Subcomponent(value); }
        }

        /// <summary>
        /// Returns 0 if null, and 1 otherwise.
        /// </summary>
        public int Count
        {
            get { return _value == null ? 0 : 1; }
        }

        /// <summary>
        /// Return an enumerable with the content inside.
        /// </summary>
        public IEnumerableIndexable<int, string> Values
        {
            get { return new WrapperEnumerable<string>(i => _value, (i, v) => { }, () => Count, 1); }
        }
    }
}