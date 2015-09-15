using System;
using System.Collections.Generic;
using NextLevelSeven.Conversion;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Provides HL7 value conversion.
    /// </summary>
    internal sealed class NativeCodec : IEncodedTypeConverter
    {
        /// <summary>
        ///     Create a codec that references the specified element's data.
        /// </summary>
        /// <param name="baseElement">Element to reference.</param>
        public NativeCodec(INativeElement baseElement)
        {
            BaseElement = baseElement;
        }

        /// <summary>
        ///     Referenced element.
        /// </summary>
        private INativeElement BaseElement { get; set; }

        /// <summary>
        ///     Get or set the element's value as a date.
        /// </summary>
        public DateTime? Date
        {
            get { return DateTimeConverter.ConvertToDate(BaseElement.Value); }
            set { BaseElement.Value = DateTimeConverter.ConvertFromDate(value); }
        }

        /// <summary>
        ///     Get the element's values as dates.
        /// </summary>
        public IIndexedEncodedTypeConverter<DateTime?> Dates
        {
            get
            {
                return new IndexedEncodedTypeConverter<DateTime?>(BaseElement, DateTimeConverter.ConvertToDate,
                    DateTimeConverter.ConvertFromDate);
            }
        }

        /// <summary>
        ///     Get or set the element's value as a date/time.
        /// </summary>
        public DateTimeOffset? DateTime
        {
            get { return DateTimeConverter.ConvertToDateTime(BaseElement.Value); }
            set { BaseElement.Value = DateTimeConverter.ConvertFromDateTime(value); }
        }

        /// <summary>
        ///     Get the element's values as date/times.
        /// </summary>
        public IIndexedEncodedTypeConverter<DateTimeOffset?> DateTimes
        {
            get
            {
                return new IndexedEncodedTypeConverter<DateTimeOffset?>(BaseElement, DateTimeConverter.ConvertToDateTime,
                    DateTimeConverter.ConvertFromDateTime);
            }
        }

        /// <summary>
        ///     Get or set the element's value as a decimal.
        /// </summary>
        public decimal? Decimal
        {
            get { return NumberConverter.ConvertToDecimal(BaseElement.Value); }
            set { BaseElement.Value = NumberConverter.ConvertFromDecimal(value); }
        }

        /// <summary>
        ///     Get the element's values as decimals.
        /// </summary>
        public IIndexedEncodedTypeConverter<decimal?> Decimals
        {
            get
            {
                return new IndexedEncodedTypeConverter<decimal?>(BaseElement, NumberConverter.ConvertToDecimal,
                    NumberConverter.ConvertFromDecimal);
            }
        }

        /// <summary>
        ///     Get or set the element's value as formatted text.
        /// </summary>
        public IEnumerable<string> FormattedText
        {
            get { return TextConverter.ConvertToFormattedText(BaseElement.Value, BaseElement.Delimiter); }
            set { BaseElement.Value = TextConverter.ConvertFromFormattedText(value, BaseElement.Delimiter); }
        }

        /// <summary>
        ///     Get or set the element's value as an integer.
        /// </summary>
        public int? Int
        {
            get { return NumberConverter.ConvertToInt(BaseElement.Value); }
            set { BaseElement.Value = NumberConverter.ConvertFromInt(value); }
        }

        /// <summary>
        ///     Get the element's values as integers.
        /// </summary>
        public IIndexedEncodedTypeConverter<int?> Ints
        {
            get
            {
                return new IndexedEncodedTypeConverter<int?>(BaseElement, NumberConverter.ConvertToInt,
                    NumberConverter.ConvertFromInt);
            }
        }

        /// <summary>
        ///     Get or set the element's value as a string.
        /// </summary>
        public string String
        {
            get { return TextConverter.ConvertToString(BaseElement.Value); }
            set { BaseElement.Value = TextConverter.ConvertFromString(value); }
        }

        /// <summary>
        ///     Get the element's values as strings.
        /// </summary>
        public IIndexedEncodedTypeConverter<string> Strings
        {
            get
            {
                return new IndexedEncodedTypeConverter<string>(BaseElement, TextConverter.ConvertToString,
                    TextConverter.ConvertFromString);
            }
        }

        // Todo: escape and format
        /// <summary>
        ///     Get or set the element's value as a text field.
        /// </summary>
        public string TextField
        {
            get { return BaseElement.Value; }
            set { BaseElement.Value = value; }
        }

        /// <summary>
        ///     Get or set the element's value as a time.
        /// </summary>
        public TimeSpan? Time
        {
            get { return DateTimeConverter.ConvertToTime(BaseElement.Value); }
            set { BaseElement.Value = DateTimeConverter.ConvertFromTime(value); }
        }

        /// <summary>
        ///     Get the element's values as times.
        /// </summary>
        public IIndexedEncodedTypeConverter<TimeSpan?> Times
        {
            get
            {
                return new IndexedEncodedTypeConverter<TimeSpan?>(BaseElement, DateTimeConverter.ConvertToTime,
                    DateTimeConverter.ConvertFromTime);
            }
        }
    }
}