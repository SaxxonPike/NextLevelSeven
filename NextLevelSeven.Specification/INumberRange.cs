namespace NextLevelSeven.Specification
{
    /// <summary>
    ///     Contains information about a numeric range in the HL7 specification. (NR)
    /// </summary>
    public interface INumberRange : ISpecificationElement
    {
        /// <summary>
        ///     Low value of the range, as a number.
        /// </summary>
        decimal? LowValue { get; set; }

        /// <summary>
        ///     Low value of the range, as a string.
        /// </summary>
        string LowValueData { get; set; }

        /// <summary>
        ///     High value of the range, as a number.
        /// </summary>
        decimal? HighValue { get; set; }

        /// <summary>
        ///     High value of the range, as a string.
        /// </summary>
        string HighValueData { get; set; }
    }
}