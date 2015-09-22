using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Core
{
    internal interface IEncodedElement : IElement
    {
        EncodingConfigurationBase EncodingConfiguration { get; }
    }
}
