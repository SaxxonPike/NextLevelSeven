using System;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     An encoding configuration that gets its values from an HL7 message.
    /// </summary>
    internal class ProxyEncodingConfiguration : EncodingConfiguration
    {
        private readonly Func<char> _getComponent;
        private readonly Func<char> _getEscape;
        private readonly Func<char> _getField;
        private readonly Func<char> _getRepetition;
        private readonly Func<char> _getSubcomponent;

        /// <summary>
        ///     Create an encoding configuration from a message or segment.
        /// </summary>
        /// <param name="field">Function to get the field delimiter character.</param>
        /// <param name="escape">Function to get the escape character.</param>
        /// <param name="repetition">Function to get the repetition character.</param>
        /// <param name="component">Function to get the component separator character.</param>
        /// <param name="subcomponent">Function to get the subcomponent separator character.</param>
        public ProxyEncodingConfiguration(Func<char> field, Func<char> escape, Func<char> repetition,
            Func<char> component, Func<char> subcomponent)
        {
            _getEscape = escape;
            _getField = field;
            _getRepetition = repetition;
            _getComponent = component;
            _getSubcomponent = subcomponent;
        }

        /// <summary>
        ///     Get the delimiter character used to split components.
        /// </summary>
        public override char ComponentDelimiter
        {
            get { return _getComponent(); }
            protected set { }
        }

        /// <summary>
        ///     Get the escape character used to mark encoded sequences.
        /// </summary>
        public override char EscapeDelimiter
        {
            get { return _getEscape(); }
            protected set { }
        }

        /// <summary>
        ///     Get the character used to separate fields.
        /// </summary>
        public override char FieldDelimiter
        {
            get { return _getField(); }
            protected set { }
        }

        /// <summary>
        ///     Get the repetition character used to separate multiple data in the same field.
        /// </summary>
        public override char RepetitionDelimiter
        {
            get { return _getRepetition(); }
            protected set { }
        }

        /// <summary>
        ///     Get the delimiter character used to split subcomponents.
        /// </summary>
        public override char SubcomponentDelimiter
        {
            get { return _getSubcomponent(); }
            protected set { }
        }

        /// <summary>
        ///     Prevent the value initialization process.
        /// </summary>
        protected override void Initialize()
        {
        }
    }
}