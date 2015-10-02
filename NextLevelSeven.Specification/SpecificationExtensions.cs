namespace NextLevelSeven.Specification
{
    /// <summary>Extensions for specification classes.</summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        ///     Return whether or not the specified number is within this number range (inclusive). Null values will always
        ///     return false.
        /// </summary>
        /// <param name="range">Range to check.</param>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the value falls within the range, false otherwise.</returns>
        public static bool Contains(this INumberRange range, decimal? value)
        {
            if (!value.HasValue)
            {
                return false;
            }
            if (range.LowValue.HasValue && value < range.LowValue.Value)
            {
                return false;
            }
            if (range.HighValue.HasValue && value > range.HighValue.Value)
            {
                return false;
            }
            return true;
        }
    }
}