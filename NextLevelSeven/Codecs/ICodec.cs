using System;

namespace NextLevelSeven.Codecs
{
    public interface ICodec
    {
        /// <summary>
        ///     Get or set element data as a date only.
        /// </summary>
        DateTime? Date { get; set; }

        /// <summary>
        ///     Get all descendant element data as dates only.
        /// </summary>
        IIndexedCodec<DateTime?> Dates { get; }

        /// <summary>
        ///     Get or set element data as a date/time.
        /// </summary>
        DateTimeOffset? DateTime { get; set; }

        /// <summary>
        ///     Get all descendant element data as date/time.
        /// </summary>
        IIndexedCodec<DateTimeOffset?> DateTimes { get; }

        /// <summary>
        ///     Get or set element data as a decimal number.
        /// </summary>
        decimal? Decimal { get; set; }

        /// <summary>
        ///     Get all descendant element data as decimal numbers.
        /// </summary>
        IIndexedCodec<decimal?> Decimals { get; }

        /// <summary>
        ///     Get or set formatted text.
        /// </summary>
        string FormattedText { get; set; }

        /// <summary>
        ///     Get or set element data as an integer.
        /// </summary>
        int? Int { get; set; }

        /// <summary>
        ///     Get all descendant element data as integers.
        /// </summary>
        IIndexedCodec<int?> Ints { get; }

        /// <summary>
        ///     Get or set element data as a string.
        /// </summary>
        string String { get; set; }

        /// <summary>
        ///     Get or set all descendant element data as strings.
        /// </summary>
        IIndexedCodec<string> Strings { get; }

        /// <summary>
        ///     Get or set data as a text field.
        /// </summary>
        string TextField { get; set; }

        /// <summary>
        ///     Get or set element data as a time only.
        /// </summary>
        TimeSpan? Time { get; set; }

        /// <summary>
        ///     Get all descendant element data as times only.
        /// </summary>
        IIndexedCodec<TimeSpan?> Times { get; }
    }
}