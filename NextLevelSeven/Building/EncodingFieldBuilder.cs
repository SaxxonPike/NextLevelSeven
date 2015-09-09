using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    /// <summary>
    /// A field builder for encoding characters.
    /// </summary>
    internal sealed class EncodingFieldBuilder : FieldBuilder
    {
        /// <summary>
        ///     Create a field builder with the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        internal EncodingFieldBuilder(BuilderBase builder)
            : base(builder)
        {
        }

        /// <summary>
        ///     Internal value.
        /// </summary>
        private string _value;

        /// <summary>
        ///     Get a descendant field repetition builder.
        /// </summary>
        /// <param name="index">Index within the field to get the builder from.</param>
        /// <returns>Field repetition builder for the specified index.</returns>
        override public RepetitionBuilder this[int index]
        {
            get { throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided); }
        }

        /// <summary>
        ///     Get the number of field repetitions in this field, including field repetitions with no content.
        /// </summary>
        override public int Count
        {
            get { return Value.Length; }
        }

        /// <summary>
        ///     Get or set field repetition content within this field.
        /// </summary>
        override public IEnumerableIndexable<int, string> Values
        {
            get { return new WrapperEnumerable<string>(OnReadValue, OnWriteValue, OnGetCount); }
        }

        /// <summary>
        ///     Set this field's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        override public FieldBuilder Field(string value)
        {
            Value = value ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Set the contents of this field.
        /// </summary>
        /// <param name="repetition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override FieldBuilder FieldRepetition(int repetition, string value)
        {
            if (repetition > 1)
            {
                throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }

        /// <summary>
        /// Get the number of encoding characters.
        /// </summary>
        /// <returns>Number of encoding characters.</returns>
        private int OnGetCount()
        {
            if (_value == null)
            {
                return 4;
            }

            var valueLength = _value.Length;
            if (valueLength > 4)
            {
                return valueLength;
            }
            return 4;
        }

        /// <summary>
        /// Get an encoding character at the specified index.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <returns>Encoding character.</returns>
        private string OnReadValue(int index)
        {
            return Value.Substring(index, 1);
        }

        /// <summary>
        /// Set an encoding character at the specified index.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <param name="value">Encoding character.</param>
        private void OnWriteValue(int index, string value)
        {
            throw new BuilderException(ErrorCode.DescendantElementsCannotBeModified);
        }

        /// <summary>
        ///     Copy the contents of this builder to a string.
        /// </summary>
        /// <returns>Converted field.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        ///     Get or set this fixed field's value.
        /// </summary>
        public string Value
        {
            get
            {
                var result = new StringBuilder();
                result.Append(ComponentDelimiter);
                result.Append(RepetitionDelimiter);
                result.Append(EscapeDelimiter);
                result.Append(SubcomponentDelimiter);
                if (_value != null && _value.Length > 4)
                {
                    result.Append(_value.Substring(4));
                }
                return result.ToString();
            }
            set
            {
                _value = value;
                if (value == null)
                {
                    return;
                }

                ComponentDelimiter = _value.Length >= 1 ? _value[0] : '^';
                RepetitionDelimiter = _value.Length >= 2 ? _value[1] : '~';
                EscapeDelimiter = _value.Length >= 3 ? _value[2] : '\\';
                SubcomponentDelimiter = _value.Length >= 4 ? _value[3] : '&';
            }
        }
    }
}
