using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Codecs
{
    internal class Codec : ICodec
    {
        public Codec(IElement baseElement)
        {
            BaseElement = baseElement;
        }

        IElement BaseElement
        {
            get;
            set;
        }

        static public string ConvertFromDate(DateTime? input)
        {
            if (!input.HasValue)
            {
                return null;
            }

            return input.Value.ToString("yyyyMMdd");
        }

        static public string ConvertFromDateTime(DateTimeOffset? input)
        {
            if (!input.HasValue)
            {
                return null;
            }

            var offset = input.Value.Offset;
            var time = input.Value;

            return time.ToString("yyyyMMddHHmmss") + offset.ToString((offset < TimeSpan.Zero ? "\\-" : "\\+") + "hhmm");
        }

        static public string ConvertFromDecimal(decimal? input)
        {
            if (!input.HasValue)
            {
                return null;
            }
            return input.Value.ToString(CultureInfo.InvariantCulture);
        }

        static public string ConvertFromInt(int? input)
        {
            if (!input.HasValue)
            {
                return null;
            }
            return input.Value.ToString(CultureInfo.InvariantCulture);
        }

        static public string ConvertFromString(string input)
        {
            return input;
        }

        static public string ConvertFromTime(TimeSpan? input)
        {
            if (!input.HasValue)
            {
                return null;
            }

            return input.Value.ToString("HHmmss");
        }

        static public DateTime? ConvertToDate(string input)
        {
            var dto = ConvertToDateTime(input);
            if (dto.HasValue)
            {
                return dto.Value.Date;
            }
            return null;
        }

        static public DateTimeOffset? ConvertToDateTime(string input)
        {
            int length = input.Length;

            if (length < 4)
            {
                return null;
            }

            var year = int.Parse(input.Substring(0, 4));
            var month = (length >= 6) ? int.Parse(input.Substring(4, 2)) : 1;
            var day = (length >= 8) ? int.Parse(input.Substring(6, 2)) : 1;
            var hour = (length >= 10) ? int.Parse(input.Substring(8, 2)) : 1;
            var minute = (length >= 12) ? int.Parse(input.Substring(10, 2)) : 1;
            var timeZoneLength = 0;
            TimeSpan? timeZone = null;

            if (length > 5)
            {
                var timeZoneIndicator = input[length - 5];
                if (timeZoneIndicator == '+' || timeZoneIndicator == '-')
                {
                    timeZoneLength = 5;
                    int timezoneHours = int.Parse(input.Substring(length - 5, 3));
                    int timezoneMinutes = int.Parse(input.Substring(length - 2, 2));
                    timeZone = new TimeSpan(timezoneHours, timezoneMinutes, 0);
                }
            }

            var second = (length >= 14 + timeZoneLength) ? decimal.Parse(input.Substring(12, length - (12 + timeZoneLength))) : 0;
            var secondValue = (int)Math.Round(second);
            var millisecond = (int)(second - secondValue);

            if (timeZone.HasValue)
            {
                return new DateTimeOffset(year, month, day, hour, minute, secondValue, millisecond, timeZone.Value);
            }
            return new DateTime(year, month, day, hour, minute, secondValue, millisecond);
        }

        static public decimal? ConvertToDecimal(string input)
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

        static public int? ConvertToInt(string input)
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

        static public string ConvertToString(string input)
        {
            return input;
        }

        static public TimeSpan? ConvertToTime(string input)
        {
            var dto = ConvertToDateTime(input);
            if (dto.HasValue)
            {
                return dto.Value.TimeOfDay;
            }
            return null;
        }

        public DateTime? Date
        {
            get { return ConvertToDate(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromDate(value); }
        }

        public IIndexedCodec<DateTime?> Dates
        {
            get { return new IndexedCodec<DateTime?>(BaseElement, ConvertToDate, ConvertFromDate); }
        }

        public DateTimeOffset? DateTime
        {
            get { return ConvertToDateTime(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromDateTime(value); }
        }

        public IIndexedCodec<DateTimeOffset?> DateTimes
        {
            get { return new IndexedCodec<DateTimeOffset?>(BaseElement, ConvertToDateTime, ConvertFromDateTime); }
        }

        public decimal? Decimal
        {
            get { return ConvertToDecimal(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromDecimal(value); }
        }

        public IIndexedCodec<decimal?> Decimals
        {
            get { return new IndexedCodec<decimal?>(BaseElement, ConvertToDecimal, ConvertFromDecimal); }
        }

        // TODO: escape and format
        public string FormattedText
        {
            get { return BaseElement.Value; }
            set { BaseElement.Value = value; }
        }

        public int? Int
        {
            get { return ConvertToInt(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromInt(value); }
        }

        public IIndexedCodec<int?> Ints
        {
            get { return new IndexedCodec<int?>(BaseElement, ConvertToInt, ConvertFromInt); }
        }

        public string String
        {
            get { return ConvertToString(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromString(value); }
        }

        public IIndexedCodec<string> Strings
        {
            get { return new IndexedCodec<string>(BaseElement, ConvertToString, ConvertFromString); }
        }

        // Todo: escape and format
        public string TextField
        {
            get { return BaseElement.Value; }
            set { BaseElement.Value = value; }
        }

        public TimeSpan? Time
        {
            get { return ConvertToTime(BaseElement.Value); }
            set { BaseElement.Value = ConvertFromTime(value); }
        }

        public IIndexedCodec<TimeSpan?> Times
        {
            get { return new IndexedCodec<TimeSpan?>(BaseElement, ConvertToTime, ConvertFromTime); }
        }
    }
}
