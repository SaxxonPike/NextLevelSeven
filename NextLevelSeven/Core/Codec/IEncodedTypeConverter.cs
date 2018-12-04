using System;
using System.Collections.Generic;

namespace NextLevelSeven.Core.Codec
{
    /// <summary>Conversion methods for HL7 data values.</summary>
    public interface IEncodedTypeConverter
    {
        /// <summary>Get or set element data as a date only.</summary>
        DateTime? Date { get; set; }

        /// <summary>Get all descendant element data as dates only.</summary>
        IIndexedEncodedTypeConverter<DateTime?> Dates { get; }

        /// <summary>Get or set element data as a date/time.</summary>
        DateTimeOffset? DateTime { get; set; }

        /// <summary>Get all descendant element data as date/time.</summary>
        IIndexedEncodedTypeConverter<DateTimeOffset?> DateTimes { get; }

        /// <summary>Get or set element data as a decimal number.</summary>
        decimal? Decimal { get; set; }

        /// <summary>Get all descendant element data as decimal numbers.</summary>
        IIndexedEncodedTypeConverter<decimal?> Decimals { get; }

        /// <summary>Get or set formatted text.</summary>
        IEnumerable<string> FormattedText { get; set; }

        /// <summary>Get or set element data as an integer.</summary>
        int? Integer { get; set; }

        /// <summary>Get all descendant element data as integers.</summary>
        IIndexedEncodedTypeConverter<int?> Integers { get; }

        /// <summary>Get or set element data as a string.</summary>
        string String { get; set; }

        /// <summary>Get or set all descendant element data as strings.</summary>
        IIndexedEncodedTypeConverter<string> Strings { get; }

        /// <summary>Get or set data as a text field.</summary>
        string TextField { get; set; }

        /// <summary>Get or set element data as a time only.</summary>
        TimeSpan? Time { get; set; }

        /// <summary>Get all descendant element data as times only.</summary>
        IIndexedEncodedTypeConverter<TimeSpan?> Times { get; }
    }
}