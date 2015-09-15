using NextLevelSeven.Core.Specification;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Extensions to the IElement interface.
    /// </summary>
    static public class ElementExtensions
    {
        /// <summary>
        ///     Get the element expressed as an HL7 address type. (AD)
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Address interpreter.</returns>
        static public IAddress AsAddress(this IElement element)
        {
            return new AddressElement(element);
        }

        /// <summary>
        ///     Get the element expressed as an HL7 coded element type. (CE)
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Coded element interpreter.</returns>
        static public ICodedElement AsCodedElement(this IElement element)
        {
            return new CodedElementElement(element);
        }

        /// <summary>
        ///     Get the element expressed as an HL7 number range type. (NR)
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Number range interpreter.</returns>
        static public INumberRange AsNumberRange(this IElement element)
        {
            return new NumberRangeElement(element);
        }
    }
}
