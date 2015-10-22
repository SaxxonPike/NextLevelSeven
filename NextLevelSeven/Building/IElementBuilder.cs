using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Building
{
    /// <summary>Base interface for element builders, which expose writeable encoding configuration.</summary>
    public interface IElementBuilder : IElement
    {
        /// <summary>Get the encoding used by this builder.</summary>
        new IEncoding Encoding { get; }
    }
}