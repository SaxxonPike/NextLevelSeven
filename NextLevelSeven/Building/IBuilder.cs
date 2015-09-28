using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Building
{
    /// <summary>Base interface for element builders, which expose writeable encoding configuration.</summary>
    public interface IBuilder : IElement
    {
        /// <summary>Get the encoding used by this builder.</summary>
        new IEncoding Encoding { get; }

        /// <summary>
        ///     Move the element to another index within its ancestor.
        /// </summary>
        /// <param name="index">New index.</param>
        void MoveToIndex(int index);
    }
}