using System.Globalization;

namespace NextLevelSeven.Conversion
{
    /// <summary>Conversion methods for Integer and Decimal types.</summary>
    public static class NumberConverter
    {
        /// <summary>Convert from decimal value to HL7 number.</summary>
        /// <param name="input">Decimal value to convert.</param>
        /// <returns>Converted decimal value.</returns>
        public static string ConvertFromDecimal(decimal? input)
        {
            return input?.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Convert from integer value to HL7 integer.</summary>
        /// <param name="input">Integer value to convert.</param>
        /// <returns>Converted integer value.</returns>
        public static string ConvertFromInt(int? input)
        {
            return input?.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Convert HL7 number to decimal value.</summary>
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

        /// <summary>Convert HL7 integer to standard integer.</summary>
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
                var decimalValue = ConvertToDecimal(input);
                if (decimalValue.HasValue)
                {
                    return (int) decimalValue;
                }
                return null;
            }

            return output;
        }
    }
}