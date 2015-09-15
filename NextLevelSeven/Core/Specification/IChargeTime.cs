using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core.Specification
{
    /// <summary>
    ///     Stores information about charge time in the HL7 specification. (CCD)
    /// </summary>
    public interface IChargeTime : ISpecificationElement
    {
        /// <summary>
        ///     Event that caused the charge. (CCD.1)
        /// </summary>
        string InvocationEvent { get; set; }

        /// <summary>
        ///     Date and time of the charge, as a date/time. (CCD.2)
        /// </summary>
        DateTimeOffset? DateTime { get; set; }

        /// <summary>
        ///     Date and time of the charge, as a string. (CCD.2)
        /// </summary>
        string DateTimeData { get; set; }
    }
}
