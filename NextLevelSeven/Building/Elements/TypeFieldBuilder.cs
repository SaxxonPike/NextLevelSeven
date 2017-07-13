using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>A fixed field builder that notifies a segment builder when its value has changed.</summary>
    internal sealed class TypeFieldBuilder : StaticValueFieldBuilder
    {
        /// <summary>Method to call when this field changes.</summary>
        private readonly ProxyChangePendingNotifier<string> _onTypeFieldChangedHandler;

        /// <summary>Internal value.</summary>
        private string _value;

        /// <summary>Create a field builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="onTypeFieldChangedHandler">Method to call when the type field has changed.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal TypeFieldBuilder(Builder builder, ProxyChangePendingNotifier<string> onTypeFieldChangedHandler,
            int index)
            : base(builder, index)
        {
            _onTypeFieldChangedHandler = onTypeFieldChangedHandler;
        }

        /// <summary>Get or set the field type value.</summary>
        public override string Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                var newValue = value;
                _onTypeFieldChangedHandler(oldValue, newValue);
                _value = newValue;
            }
        }
    }
}