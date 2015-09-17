using NextLevelSeven.Specification.Elements;

namespace NextLevelSeven.Specification.Conversion
{
    /// <summary>
    ///     Converter for HL7 address type.
    /// </summary>
    public static class AddressTypeConverter
    {
        /// <summary>
        ///     Get the raw value for the specified address type.
        /// </summary>
        /// <param name="addressType">Address type.</param>
        /// <returns>HL7 value.</returns>
        public static string ConvertFromTable(AddressType addressType)
        {
            switch (addressType)
            {
                case AddressType.Birth:
                    return "N";
                case AddressType.CountryOfOrigin:
                    return "F";
                case AddressType.CurrentOrTemporary:
                    return "C";
                case AddressType.FirmOrBusiness:
                    return "B";
                case AddressType.Home:
                    return "H";
                case AddressType.Mailing:
                    return "M";
                case AddressType.Office:
                    return "O";
                case AddressType.Permanent:
                    return "P";
            }
            return null;
        }

        /// <summary>
        ///     Get the address type from a raw value.
        /// </summary>
        /// <param name="addressType">Address type.</param>
        /// <returns>Interpreted value.</returns>
        public static AddressType ConvertToTable(string addressType)
        {
            if (addressType == null)
            {
                return AddressType.Null;
            }

            switch (addressType.ToUpperInvariant().Trim())
            {
                case "B":
                    return AddressType.FirmOrBusiness;
                case "C":
                    return AddressType.CurrentOrTemporary;
                case "F":
                    return AddressType.CountryOfOrigin;
                case "H":
                    return AddressType.Home;
                case "M":
                    return AddressType.Mailing;
                case "N":
                    return AddressType.Birth;
                case "O":
                    return AddressType.Office;
                case "P":
                    return AddressType.Permanent;
            }

            return AddressType.Null;
        }
    }
}