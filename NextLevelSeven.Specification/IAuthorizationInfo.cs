using System;

namespace NextLevelSeven.Specification
{
    /// <summary>Stores authorization information in the HL7 specification. (AUI)</summary>
    public interface IAuthorizationInfo : ISpecificationElement
    {
        /// <summary>Authorization number. (AUI.1)</summary>
        string AuthorizationNumber { get; set; }

        /// <summary>Date of authorization, as a date/time. (AUI.2)</summary>
        DateTime? Date { get; set; }

        /// <summary>Date of authorization, as a string. (AUI.2)</summary>
        string DateData { get; set; }

        /// <summary>Source of authorization. (AUI.3)</summary>
        string Source { get; set; }
    }
}