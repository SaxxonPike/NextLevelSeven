using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>A field builder for encoding characters.</summary>
    internal sealed class EncodingFieldBuilder : StaticValueFieldBuilder
    {
        /// <summary>Internal value.</summary>
        private string _value;

        /// <summary>Create a field builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal EncodingFieldBuilder(Builder builder, int index)
            : base(builder, index)
        {
        }

        /// <summary>Get the number of field repetitions in this field, including field repetitions with no content.</summary>
        public override int ValueCount => Value.Length;

        /// <summary>Get or set field repetition content within this field.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = Value.Length;
                for (var i = 0; i < count; i++)
                {
                    yield return new string(_value[i], 1);
                }
            }
            set => SetField(string.Concat(value));
        }

        /// <summary>Get or set this fixed field's value.</summary>
        public override string Value
        {
            get
            {
                var result = new StringBuilder();

                if (ComponentDelimiter != '\0')
                {
                    result.Append(ComponentDelimiter);
                }
                if (RepetitionDelimiter != '\0')
                {
                    result.Append(RepetitionDelimiter);
                }
                if (EscapeCharacter != '\0')
                {
                    result.Append(EscapeCharacter);
                }
                if (SubcomponentDelimiter != '\0')
                {
                    result.Append(SubcomponentDelimiter);
                }

                if (_value != null && _value.Length > 4)
                {
                    result.Append(_value.Substring(4));
                }

                return HL7.NullValues.Contains(result.ToString())
                    ? null
                    : result.ToString();
            }
            set
            {
                _value = value;
                ComponentDelimiter = _value.Length >= 1 ? _value[0] : '\0';
                RepetitionDelimiter = _value.Length >= 2 ? _value[1] : '\0';
                EscapeCharacter = _value.Length >= 3 ? _value[2] : '\0';
                SubcomponentDelimiter = _value.Length >= 4 ? _value[3] : '\0';
            }
        }
    }
}