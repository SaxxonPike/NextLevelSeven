namespace NextLevelSeven.Utility
{
    /// <summary>
    /// An indexable collection.
    /// </summary>
    /// <typeparam name="TIndex">Type of indexer.</typeparam>
    /// <typeparam name="TItem">Type of contained items.</typeparam>
    public interface IIndexable<in TIndex, TItem>
    {
        /// <summary>
        /// Get or set the item at the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Item.</returns>
        TItem this[TIndex index] { get; set; }
    }
}
