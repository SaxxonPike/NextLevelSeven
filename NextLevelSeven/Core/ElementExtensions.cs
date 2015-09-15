using NextLevelSeven.Core.Specification;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Extensions to the IElement interface.
    /// </summary>
    static public class ElementExtensions
    {
        /// <summary>
        ///     Get the element expressed as an HL7 address type.
        /// </summary>
        /// <param name="element">Element to interpret.</param>
        /// <returns>Address interpreter.</returns>
        static public IAddress AsAddress(this IElement element)
        {
            return new AddressElement(element);
        }
    }
}
