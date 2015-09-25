namespace NextLevelSeven.Specification
{
    /// <summary>Represents a coded element in the HL7 specification. (CE)</summary>
    public interface ICodedElement : ISpecificationElement
    {
        /// <summary>Identifier of the coded element. (CE.1)</summary>
        string Identifier { get; set; }

        /// <summary>Text. (CE.2)</summary>
        string Text { get; set; }

        /// <summary>Name of the coding system used. (CE.3)</summary>
        string CodingSystemName { get; set; }

        /// <summary>Alternate identifier of the coded element. (CE.4)</summary>
        string AlternateIdentifier { get; set; }

        /// <summary>Alternate text. (CE.5)</summary>
        string AlternateText { get; set; }

        /// <summary>Name of the alternate coding system used. (CE.6)</summary>
        string AlternateCodingSystemName { get; set; }
    }
}