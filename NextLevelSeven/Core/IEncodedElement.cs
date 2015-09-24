using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Describes an element that also has an encoding configuration.
    /// </summary>
    internal interface IEncodedElement : IElement
    {
        /// <summary>
        ///     Get the element's encoding configuration.
        /// </summary>
        EncodingConfigurationBase EncodingConfiguration { get; }
    }
}