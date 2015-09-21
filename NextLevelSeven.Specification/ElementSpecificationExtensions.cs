using NextLevelSeven.Core;
using NextLevelSeven.Specification.Elements;

namespace NextLevelSeven.Specification
{
    /// <summary>
    ///     Specification-specific extensions to the IElement interface.
    /// </summary>
    public static class ElementSpecificationExtensions
    {
        /// <summary>
        ///     Get the element expressed as an HL7 address type. (AD)
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Address interpreter.</returns>
        public static IAddress AsAddress(this IElement element)
        {
            return new AddressElement(element);
        }

        /// <summary>
        ///     Get the element expressed as an HL7 coded element type. (CE)
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Coded element interpreter.</returns>
        public static ICodedElement AsCodedElement(this IElement element)
        {
            return new CodedElementElement(element);
        }

        /// <summary>
        ///     Get the element expressed as an HL7 number range type. (NR)
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Number range interpreter.</returns>
        public static INumberRange AsNumberRange(this IElement element)
        {
            return new NumberRangeElement(element);
        }
    }
}