using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     A field builder for encoding characters.
    /// </summary>
    internal sealed class EncodingFieldBuilder : FieldBuilder
    {
        /// <summary>
        ///     Internal value.
        /// </summary>
        private string _value;

        /// <summary>
        ///     Create a field builder with the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal EncodingFieldBuilder(BuilderBase builder, int index)
            : base(builder, index)
        {
        }

        /// <summary>
        ///     Get the number of field repetitions in this field, including field repetitions with no content.
        /// </summary>
        public override int ValueCount
        {
            get { return Value.Length; }
        }

        /// <summary>
        ///     Get or set field repetition content within this field.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get { return new ProxyEnumerable<string>(OnReadValue, OnWriteValue, OnGetCount); }
            set { SetField(string.Concat(value)); }
        }

        /// <summary>
        ///     Get or set this fixed field's value.
        /// </summary>
        public override string Value
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

        /// <summary>
        ///     Encoding character fields cannot have repetitions; this method throws unconditionally.
        /// </summary>
        /// <param name="index">Not used.</param>
        /// <returns>Nothing.</returns>
        protected override RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
        }

        /// <summary>
        ///     Set this field's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public override IFieldBuilder SetField(string value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        ///     Set the contents of this field.
        /// </summary>
        /// <param name="repetition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IFieldBuilder SetFieldRepetition(int repetition, string value)
        {
            if (repetition > 1)
            {
                throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }

        /// <summary>
        ///     Get the number of encoding characters.
        /// </summary>
        /// <returns>Number of encoding characters.</returns>
        private int OnGetCount()
        {
            if (_value == null)
            {
                return 4;
            }

            var valueLength = _value.Length;
            return valueLength > 4
                ? valueLength
                : 4;
        }

        /// <summary>
        ///     Get an encoding character at the specified index.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <returns>Encoding character.</returns>
        private string OnReadValue(int index)
        {
            return Value.Substring(index, 1);
        }

        /// <summary>
        ///     Set an encoding character at the specified index.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <param name="value">Encoding character.</param>
        private static void OnWriteValue(int index, string value)
        {
            throw new BuilderException(ErrorCode.DescendantElementsCannotBeModified);
        }

        /// <summary>
        ///     Get descendant elements.
        /// </summary>
        /// <returns>Descendant elements.</returns>
        protected override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }
    }
}