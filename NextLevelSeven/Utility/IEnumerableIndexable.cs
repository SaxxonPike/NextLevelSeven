using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    /// <summary>
    ///     An indexable, enumerable collection.
    /// </summary>
    /// <typeparam name="TIndex">Type of indexer.</typeparam>
    /// <typeparam name="TItem">Type of contained items.</typeparam>
    public interface IEnumerableIndexable<in TIndex, TItem> : IEnumerable<TItem>, IIndexable<TIndex, TItem>
    {
        /// <summary>
        ///     Copy the contained items to another array, starting at the specified index.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="arrayIndex">Index on the target array to start.</param>
        void CopyTo(TItem[] array, int arrayIndex);

        /// <summary>
        ///     Copy the contained items to a new array.
        /// </summary>
        /// <returns>Array containing the items.</returns>
        TItem[] ToArray();
    }
}