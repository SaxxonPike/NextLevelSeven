using System;
using System.Collections.Generic;

namespace NextLevelSeven.Core.Codec
{
    /// <summary>Conversion methods for HL7 data values.</summary>
    public interface IEncodedTypeConverter
    {
        /// <summary>Get or set element data as a date only.</summary>
        DateTime? AsDate { get; set; }

        /// <summary>Get all descendant element data as dates only.</summary>
        IIndexedEncodedTypeConverter<DateTime?> AsDates { get; }

        /// <summary>Get or set element data as a date/time.</summary>
        DateTimeOffset? AsDateTime { get; set; }

        /// <summary>Get all descendant element data as date/time.</summary>
        IIndexedEncodedTypeConverter<DateTimeOffset?> AsDateTimes { get; }

        /// <summary>Get or set element data as a decimal number.</summary>
        decimal? AsDecimal { get; set; }

        /// <summary>Get all descendant element data as decimal numbers.</summary>
        IIndexedEncodedTypeConverter<decimal?> AsDecimals { get; }

        /// <summary>Get or set formatted text.</summary>
        IEnumerable<string> AsFormattedText { get; set; }

        /// <summary>Get or set element data as an integer.</summary>
        int? AsInt { get; set; }

        /// <summary>Get all descendant element data as integers.</summary>
        IIndexedEncodedTypeConverter<int?> AsInts { get; }

        /// <summary>Get or set element data as a string.</summary>
        string AsString { get; set; }

        /// <summary>Get or set all descendant element data as strings.</summary>
        IIndexedEncodedTypeConverter<string> AsStrings { get; }

        /// <summary>Get or set data as a text field.</summary>
        string AsTextField { get; set; }

        /// <summary>Get or set element data as a time only.</summary>
        TimeSpan? AsTime { get; set; }

        /// <summary>Get all descendant element data as times only.</summary>
        IIndexedEncodedTypeConverter<TimeSpan?> AsTimes { get; }
    }
}