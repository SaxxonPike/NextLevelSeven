using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     A fixed field builder that notifies a segment builder when its value has changed.
    /// </summary>
    internal sealed class TypeFieldBuilder : FieldBuilder
    {
        /// <summary>
        ///     Method to call when this field changes.
        /// </summary>
        private readonly ProxyChangePendingNotifier<string> _onTypeFieldChangedHandler;

        /// <summary>
        ///     Internal value.
        /// </summary>
        private string _value;

        /// <summary>
        ///     Create a field builder with the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="onTypeFieldChangedHandler">Method to call when the type field has changed.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal TypeFieldBuilder(BuilderBase builder, ProxyChangePendingNotifier<string> onTypeFieldChangedHandler,
            int index)
            : base(builder, index)
        {
            _onTypeFieldChangedHandler = onTypeFieldChangedHandler;
        }

        /// <summary>
        ///     Get or set the field type value.
        /// </summary>
        public override string Value
        {
            get { return _value; }
            set
            {
                var oldValue = _value;
                var newValue = value;
                _onTypeFieldChangedHandler(oldValue, newValue);
                _value = newValue;
            }
        }

        /// <summary>
        ///     Type fields cannot have repetitions; this method throws unconditionally.
        /// </summary>
        /// <param name="index">Not used.</param>
        /// <returns>Nothing.</returns>
        protected override RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
        }

        /// <summary>
        ///     Set the contents of this field.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This TypeFieldBuilder.</returns>
        public override IFieldBuilder SetField(string value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        ///     Set the contents of this field.
        /// </summary>
        /// <param name="repetition">Repetition number. All values greater than one are invalid.</param>
        /// <param name="value">New value.</param>
        /// <returns>This TypeFieldBuilder.</returns>
        public override IFieldBuilder SetFieldRepetition(int repetition, string value)
        {
            if (repetition > 1)
            {
                throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }
    }
}