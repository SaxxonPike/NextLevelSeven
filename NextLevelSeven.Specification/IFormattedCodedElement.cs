using System;

namespace NextLevelSeven.Specification
{
    /// <summary>Represents a coded element with formatted values in the HL7 specification. (CF)</summary>
    public interface IFormattedCodedElement : ISpecificationElement
    {
        /// <summary>Identifier of the formatted coded element. (CF.1)</summary>
        string Identifier
        {
            get;
            set;
        }

        /// <summary>Formatted text. (CF.2)</summary>
        string Text
        {
            get;
            set;
        }

        /// <summary>Name of the coding system used. (CF.3)</summary>
        string CodingSystemName
        {
            get;
            set;
        }

        /// <summary>Alternate identifier of the formatted coded element. (CF.4)</summary>
        string AlternateIdentifier
        {
            get;
            set;
        }

        /// <summary>Alternate formatted text. (CF.5)</summary>
        string AlternateText
        {
            get;
            set;
        }

        /// <summary>Name of the alternate coding system used. (CF.6)</summary>
        string AlternateCodingSystemName
        {
            get;
            set;
        }

        /// <summary>Version ID of the coding system. (CF.7)</summary>
        string CodingSystemVersionId
        {
            get;
            set;
        }

        /// <summary>Version ID of the alternate coding system. (CF.8)</summary>
        string AlternateCodingSystemVersionId
        {
            get;
            set;
        }

        /// <summary>Original text of the coded element. (CF.9)</summary>
        string OriginalText
        {
            get;
            set;
        }

        /// <summary>Second alternative identifier of the formatted coded element. (CF.10)</summary>
        string SecondAlternativeIdentifier
        {
            get;
            set;
        }

        /// <summary>Second alternate formatted text. (CF.11)</summary>
        string SecondAlternateText
        {
            get;
            set;
        }

        /// <summary>Name of the second alternate coding system used. (CF.12)</summary>
        string SecondAlternateCodingSystemName
        {
            get;
            set;
        }

        /// <summary>Version ID of the second alternate coding system. (CF.13)</summary>
        string SecondAlternateCodingSystemVersionId
        {
            get;
            set;
        }

        /// <summary>Coding system OID. (CF.14)</summary>
        string CodingSystemOid
        {
            get;
            set;
        }

        /// <summary>Value set OID. (CF.15)</summary>
        string ValueSetOid
        {
            get;
            set;
        }

        /// <summary>Value set version ID, as a date/time. (CF.16)</summary>
        DateTimeOffset? ValueSetVersionId
        {
            get;
            set;
        }

        /// <summary>Value set version ID, as a string. (CF.16)</summary>
        string ValueSetVersionIdData
        {
            get;
            set;
        }

        /// <summary>Alternate coding system OID. (CF.17)</summary>
        string AlternateCodingSystemOid
        {
            get;
            set;
        }

        /// <summary>Alternate value set OID. (CF.18)</summary>
        string AlternateValueSetOid
        {
            get;
            set;
        }

        /// <summary>Alternate value set version ID, as a date/time. (CF.19)</summary>
        DateTimeOffset? AlternateValueSetVersionId
        {
            get;
            set;
        }

        /// <summary>Alternate value set version ID, as a string. (CF.19)</summary>
        string AlternateValueSetVersionIdData
        {
            get;
            set;
        }

        /// <summary>Second alternate coding system OID. (CF.20)</summary>
        string SecondAlternateCodingSystemOid
        {
            get;
            set;
        }

        /// <summary>Second alternate value set OID. (CF.21)</summary>
        string SecondAlternateValueSetOid
        {
            get;
            set;
        }

        /// <summary>Second alternate value set version ID, as a date/time. (CF.22)</summary>
        DateTimeOffset? SecondAlternateValueSetVersionId
        {
            get;
            set;
        }

        /// <summary>Second alternate value set version ID, as a string. (CF.22)</summary>
        string SecondAlternateValueSetVersionIdData
        {
            get;
            set;
        }
    }
}