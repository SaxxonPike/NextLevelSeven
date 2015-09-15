using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core.Specification
{
    /// <summary>
    ///     Holds information about a street address in the HL7 specification. (AD)
    /// </summary>
    public interface IAddress : ISpecificationElement
    {
        /// <summary>
        ///     Street address. (AD.1)
        /// </summary>
        string StreetAddress { get; set; }

        /// <summary>
        ///     Secondary street address information. (AD.2)
        /// </summary>
        string OtherDesignation { get; set; }

        /// <summary>
        ///     City of the address. (AD.3)
        /// </summary>
        string City { get; set; }

        /// <summary>
        ///     State or province of the address. (AD.4)
        /// </summary>
        string StateOrProvince { get; set; }

        /// <summary>
        ///     ZIP or other postal code of the address. (AD.5)
        /// </summary>
        string ZipOrPostalCode { get; set; }

        /// <summary>
        ///     Country of the address. (AD.6)
        /// </summary>
        string Country { get; set; }

        /// <summary>
        ///     HL7 address type, from a predefined list. (AD.7)
        /// </summary>
        AddressType AddressType { get; set; }

        /// <summary>
        ///     HL7 address type, as a string. (AD.7)
        /// </summary>
        string AddressTypeData { get; set; }

        /// <summary>
        ///     Additional address information. (AD.8)
        /// </summary>
        string OtherGeographicDesignation { get; set; }
    }
}
