namespace NextLevelSeven.Utility
{
    public interface IIndexable<in TIndex, TItem>
    {
        TItem this[TIndex index] { get; set; }
    }
}
