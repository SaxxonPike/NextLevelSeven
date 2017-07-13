using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Conversion
{
    /// <summary>Conversion methods for HL7 date/time values.</summary>
    public static class DateTimeConverter
    {
        /// <summary>Convert from date to HL7 date.</summary>
        /// <param name="input">Date to convert.</param>
        /// <returns>Converted date.</returns>
        public static string ConvertFromDate(DateTime? input)
        {
            return input?.ToString("yyyyMMdd");
        }

        /// <summary>Convert from date/time to HL7 date/time.</summary>
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

            return time.ToString("yyyyMMddHHmmss") + offset.ToString((offset < TimeSpan.Zero ? "\\-" : "\\+") + "hhmm");
        }

        /// <summary>Convert from time to HL7 time.</summary>
        /// <param name="input">Time to convert.</param>
        /// <returns>Converted time.</returns>
        public static string ConvertFromTime(TimeSpan? input)
        {
            return input?.ToString("HHmmss");
        }

        /// <summary>Convert from HL7 date to .NET date.</summary>
        /// <param name="input">Date to convert.</param>
        /// <returns>Converted date.</returns>
        public static DateTime? ConvertToDate(string input)
        {
            var dto = ConvertToDateTime(input);
            return dto?.Date;
        }

        /// <summary>Convert from HL7 date/time to .NET date/time.</summary>
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
                throw new ConversionException(ErrorCode.UnableToParseDate);
            }

            var year = int.Parse(input.Substring(0, 4));

            var month = length >= 6
                ? int.Parse(input.Substring(4, 2))
                : 1;

            var day = length >= 8
                ? int.Parse(input.Substring(6, 2))
                : 1;

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

            var time = length >= 10 + timeZoneLength
                ? (ConvertToTime(input.Substring(8, input.Length - timeZoneLength - 8)) ?? TimeSpan.Zero)
                : TimeSpan.Zero;

            return timeZone.HasValue
                ? new DateTimeOffset(year, month, day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds,
                    timeZone.Value)
                : new DateTime(year, month, day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }

        /// <summary>Convert HL7 time to .NET time.</summary>
        /// <param name="input">Time to convert.</param>
        /// <returns>Converted time.</returns>
        public static TimeSpan? ConvertToTime(string input)
        {
            if (input == null)
            {
                return null;
            }

            var length = input.Length;

            var hour = length >= 2
                ? int.Parse(input.Substring(0, 2))
                : 0;

            var minute = length >= 4
                ? int.Parse(input.Substring(2, 2))
                : 0;

            var second = length >= 6
                ? decimal.Parse(input.Substring(4))
                : 0;

            var secondValue = (int) Math.Round(second);
            var millisecond = (int) (second - secondValue);

            return new TimeSpan(0, hour, minute, secondValue, millisecond);
        }
    }
}