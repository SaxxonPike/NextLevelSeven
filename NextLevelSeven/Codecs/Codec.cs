using System;
using System.Globalization;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Codecs
{
    /// <summary>
    ///     Provides HL7 value conversion.
    /// </summary>
    internal sealed class Codec : ICodec
    {
        /// <summary>
        ///     Create a codec that references the specified element's data.
        /// </summary>
        /// <param name="baseElement">Element to reference.</param>
        public Codec(IElement baseElement)
        {
            BaseElement = baseElement;
        }

        /// <summary>
        ///     Referenced element.
        /// </summary>
        private IElement BaseElement { get; set; }

        /// <summary>
        ///     Get or set the element's value as a date.
        /// </summary>
        public DateTime? Date
        {
            get { return ConvertToDate(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromDate(value); }
        }

        /// <summary>
        ///     Get the element's values as dates.
        /// </summary>
        public IIndexedCodec<DateTime?> Dates
        {
            get { return new IndexedCodec<DateTime?>(BaseElement, ConvertToDate, ConvertFromDate); }
        }

        /// <summary>
        ///     Get or set the element's value as a date/time.
        /// </summary>
        public DateTimeOffset? DateTime
        {
            get { return ConvertToDateTime(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromDateTime(value); }
        }

        /// <summary>
        ///     Get the element's values as date/times.
        /// </summary>
        public IIndexedCodec<DateTimeOffset?> DateTimes
        {
            get { return new IndexedCodec<DateTimeOffset?>(BaseElement, ConvertToDateTime, ConvertFromDateTime); }
        }

        /// <summary>
        ///     Get or set the element's value as a decimal.
        /// </summary>
        public decimal? Decimal
        {
            get { return ConvertToDecimal(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromDecimal(value); }
        }

        /// <summary>
        ///     Get the element's values as decimals.
        /// </summary>
        public IIndexedCodec<decimal?> Decimals
        {
            get { return new IndexedCodec<decimal?>(BaseElement, ConvertToDecimal, ConvertFromDecimal); }
        }

        // TODO: escape and format
        /// <summary>
        ///     Get or set the element's value as formatted text.
        /// </summary>
        public string FormattedText
        {
            get { return BaseElement.Value; }
            set { BaseElement.Value = value; }
        }

        /// <summary>
        ///     Get or set the element's value as an integer.
        /// </summary>
        public int? Int
        {
            get { return ConvertToInt(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromInt(value); }
        }

        /// <summary>
        ///     Get the element's values as integers.
        /// </summary>
        public IIndexedCodec<int?> Ints
        {
            get { return new IndexedCodec<int?>(BaseElement, ConvertToInt, ConvertFromInt); }
        }

        /// <summary>
        ///     Get or set the element's value as a string.
        /// </summary>
        public string String
        {
            get { return ConvertToString(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromString(value); }
        }

        /// <summary>
        ///     Get the element's values as strings.
        /// </summary>
        public IIndexedCodec<string> Strings
        {
            get { return new IndexedCodec<string>(BaseElement, ConvertToString, ConvertFromString); }
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
            get { return ConvertToTime(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromTime(value); }
        }

        /// <summary>
        ///     Get the element's values as times.
        /// </summary>
        public IIndexedCodec<TimeSpan?> Times
        {
            get { return new IndexedCodec<TimeSpan?>(BaseElement, ConvertToTime, ConvertFromTime); }
        }

        /// <summary>
        ///     Convert from date to HL7 date.
        /// </summary>
        /// <param name="input">Date to convert.</param>
        /// <returns>Converted date.</returns>
        public static string ConvertFromDate(DateTime? input)
        {
            if (!input.HasValue)
            {
                return null;
            }

            return input.Value.ToString("yyyyMMdd");
        }

        /// <summary>
        ///     Convert from date/time to HL7 date/time.
        /// </summary>
        /// <param name="input">Date/time to convert.</param>
        /// <returns>Converted date/time.</returns>
        public static string ConvertFromDateTime(DateTimeOffset? input)
        {
            if (!input.HasValue)
            {
                return null;
            }

            var offset = input.Value.Offset;
            var time = input.Value;

            return time.ToString("yyyyMMddHHmmss") +
                   offset.ToString((offset < TimeSpan.Zero ? "\\-" : "\\+") + "hhmm");
        }

        /// <summary>
        ///     Convert from decimal value to HL7 number.
        /// </summary>
        /// <param name="input">Decimal value to convert.</param>
        /// <returns>Converted decimal value.</returns>
        public static string ConvertFromDecimal(decimal? input)
        {
            return (input.HasValue)
                ? input.Value.ToString(CultureInfo.InvariantCulture)
                : null;
        }

        /// <summary>
        ///     Convert from integer value to HL7 integer.
        /// </summary>
        /// <param name="input">Integer value to convert.</param>
        /// <returns>Converted integer value.</returns>
        public static string ConvertFromInt(int? input)
        {
            return (input.HasValue)
                ? input.Value.ToString(CultureInfo.InvariantCulture)
                : null;
        }

        /// <summary>
        ///     Convert from standard string to HL7 string.
        /// </summary>
        /// <param name="input">String value to convert.</param>
        /// <returns>Converted string.</returns>
        public static string ConvertFromString(string input)
        {
            return input;
        }

        /// <summary>
        ///     Convert from time to HL7 time.
        /// </summary>
        /// <param name="input">Time to convert.</param>
        /// <returns>Converted time.</returns>
        public static string ConvertFromTime(TimeSpan? input)
        {
            if (!input.HasValue)
            {
                return null;
            }

            return input.Value.ToString("HHmmss");
        }

        /// <summary>
        ///     Convert from HL7 date to .NET date.
        /// </summary>
        /// <param name="input">Date to convert.</param>
        /// <returns>Converted date.</returns>
        public static DateTime? ConvertToDate(string input)
        {
            var dto = ConvertToDateTime(input);
            if (dto.HasValue)
            {
                return dto.Value.Date;
            }
            return null;
        }

        /// <summary>
        ///     Convert from HL7 date/time to .NET date/time.
        /// </summary>
        /// <param name="input">Date/time to convert.</param>
        /// <returns>Converted date/time.</returns>
        public static DateTimeOffset? ConvertToDateTime(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var length = input.Length;
            if (length < 4)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.UnableToParseDate));
            }

            var year = int.Parse(input.Substring(0, 4));
            var month = (length >= 6) ? int.Parse(input.Substring(4, 2)) : 1;
            var day = (length >= 8) ? int.Parse(input.Substring(6, 2)) : 1;
            var timeZoneLength = 0;
            TimeSpan? timeZone = null;

            if (length > 5)
            {
                var timeZoneIndicator = input[length - 5];
                if (timeZoneIndicator == '+' || timeZoneIndicator == '-')
                {
                    timeZoneLength = 5;
                    var timezoneHours = int.Parse(input.Substring(length - 5, 3));
                    var timezoneMinutes = int.Parse(input.Substring(length - 2, 2));
                    timeZone = new TimeSpan(timezoneHours, timezoneMinutes, 0);
                }
            }

            var time = (length >= 10 + timeZoneLength)
                ? (ConvertToTime(input.Substring(8, input.Length - timeZoneLength - 8)) ?? TimeSpan.Zero)
                : TimeSpan.Zero;

            return (timeZone.HasValue)
                ? new DateTimeOffset(year, month, day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds,
                    timeZone.Value)
                : new DateTime(year, month, day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }

        /// <summary>
        ///     Convert HL7 number to decimal value.
        /// </summary>
        /// <param name="input">Number to convert.</param>
        /// <returns>Converted number.</returns>
        public static decimal? ConvertToDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            decimal output;
            if (!decimal.TryParse(input.Trim(), out output))
            {
                return null;
            }

            return output;
        }

        /// <summary>
        ///     Convert HL7 integer to standard integer.
        /// </summary>
        /// <param name="input">Integer to convert.</param>
        /// <returns>Converted integer.</returns>
        public static int? ConvertToInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            int output;
            if (!int.TryParse(input.Trim(), out output))
            {
                return null;
            }

            return output;
        }

        /// <summary>
        ///     Convert HL7 string to standard string.
        /// </summary>
        /// <param name="input">String to convert.</param>
        /// <returns>Converted string.</returns>
        public static string ConvertToString(string input)
        {
            return input;
        }

        /// <summary>
        ///     Convert HL7 time to .NET time.
        /// </summary>
        /// <param name="input">Time to convert.</param>
        /// <returns>Converted time.</returns>
        public static TimeSpan? ConvertToTime(string input)
        {
            if (input == null)
            {
                return null;
            }

            var length = input.Length;
            var hour = (length >= 2) ? int.Parse(input.Substring(0, 2)) : 0;
            var minute = (length >= 4) ? int.Parse(input.Substring(2, 2)) : 0;
            var second = (length >= 6) ? decimal.Parse(input.Substring(4)) : 0;
            var secondValue = (int) Math.Round(second);
            var millisecond = (int) (second - secondValue);

            return new TimeSpan(0, hour, minute, secondValue, millisecond);
        }
    }
}