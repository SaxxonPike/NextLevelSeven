using System;
using System.Collections.Generic;
using NextLevelSeven.Conversion;

namespace NextLevelSeven.Core.Codec
{
    /// <summary>Provides HL7 value conversion.</summary>
    internal sealed class EncodedTypeConverter : IEncodedTypeConverter
    {
        /// <summary>Referenced element.</summary>
        private readonly IElement _baseElement;

        /// <summary>Create a codec that references the specified element's data.</summary>
        /// <param name="baseElement">Element to reference.</param>
        public EncodedTypeConverter(IElement baseElement)
        {
            _baseElement = baseElement;
        }

        /// <summary>Get or set the element's value as a date.</summary>
        public DateTime? AsDate
        {
            get { return DateTimeConverter.ConvertToDate(_baseElement.Value); }
            set { _baseElement.Value = DateTimeConverter.ConvertFromDate(value); }
        }

        /// <summary>Get the element's values as dates.</summary>
        public IIndexedEncodedTypeConverter<DateTime?> AsDates
        {
            get
            {
                return new IndexedEncodedTypeConverter<DateTime?>(_baseElement, DateTimeConverter.ConvertToDate,
                    DateTimeConverter.ConvertFromDate);
            }
        }

        /// <summary>Get or set the element's value as a date/time.</summary>
        public DateTimeOffset? AsDateTime
        {
            get { return DateTimeConverter.ConvertToDateTime(_baseElement.Value); }
            set { _baseElement.Value = DateTimeConverter.ConvertFromDateTime(value); }
        }

        /// <summary>Get the element's values as date/times.</summary>
        public IIndexedEncodedTypeConverter<DateTimeOffset?> AsDateTimes
        {
            get
            {
                return new IndexedEncodedTypeConverter<DateTimeOffset?>(_baseElement,
                    DateTimeConverter.ConvertToDateTime,
                    DateTimeConverter.ConvertFromDateTime);
            }
        }

        /// <summary>Get or set the element's value as a decimal.</summary>
        public decimal? AsDecimal
        {
            get { return NumberConverter.ConvertToDecimal(_baseElement.Value); }
            set { _baseElement.Value = NumberConverter.ConvertFromDecimal(value); }
        }

        /// <summary>Get the element's values as decimals.</summary>
        public IIndexedEncodedTypeConverter<decimal?> AsDecimals
        {
            get
            {
                return new IndexedEncodedTypeConverter<decimal?>(_baseElement, NumberConverter.ConvertToDecimal,
                    NumberConverter.ConvertFromDecimal);
            }
        }

        /// <summary>Get or set the element's value as formatted text.</summary>
        public IEnumerable<string> AsFormattedText
        {
            get
            {
                return TextConverter.ConvertToFormattedText(_baseElement.Value, _baseElement.Delimiter,
                    _baseElement.Encoding);
            }
            set
            {
                _baseElement.Value = TextConverter.ConvertFromFormattedText(value, _baseElement.Delimiter,
                    _baseElement.Encoding);
            }
        }

        /// <summary>Get or set the element's value as an integer.</summary>
        public int? AsInt
        {
            get { return NumberConverter.ConvertToInt(_baseElement.Value); }
            set { _baseElement.Value = NumberConverter.ConvertFromInt(value); }
        }

        /// <summary>Get the element's values as integers.</summary>
        public IIndexedEncodedTypeConverter<int?> AsInts
        {
            get
            {
                return new IndexedEncodedTypeConverter<int?>(_baseElement, NumberConverter.ConvertToInt,
                    NumberConverter.ConvertFromInt);
            }
        }

        /// <summary>Get or set the element's value as a string.</summary>
        public string AsString
        {
            get { return TextConverter.ConvertToString(_baseElement.Value); }
            set { _baseElement.Value = TextConverter.ConvertFromString(value); }
        }

        /// <summary>Get the element's values as strings.</summary>
        public IIndexedEncodedTypeConverter<string> AsStrings
        {
            get
            {
                return new IndexedEncodedTypeConverter<string>(_baseElement, TextConverter.ConvertToString,
                    TextConverter.ConvertFromString);
            }
        }

        /// <summary>Get or set the element's value as a text field.</summary>
        public string AsTextField
        {
            get
            {
                return string.Join(Environment.NewLine,
                    TextConverter.ConvertToFormattedText(_baseElement.Value, _baseElement.Delimiter,
                        _baseElement.Encoding));
            }
            set
            {
                _baseElement.Value =
                    TextConverter.ConvertFromFormattedText(value.Replace(Environment.NewLine, "\xD").Split('\xD'),
                        _baseElement.Delimiter, _baseElement.Encoding);
            }
        }

        /// <summary>Get or set the element's value as a time.</summary>
        public TimeSpan? AsTime
        {
            get { return DateTimeConverter.ConvertToTime(_baseElement.Value); }
            set { _baseElement.Value = DateTimeConverter.ConvertFromTime(value); }
        }

        /// <summary>Get the element's values as times.</summary>
        public IIndexedEncodedTypeConverter<TimeSpan?> AsTimes
        {
            get
            {
                return new IndexedEncodedTypeConverter<TimeSpan?>(_baseElement, DateTimeConverter.ConvertToTime,
                    DateTimeConverter.ConvertFromTime);
            }
        }
    }
}