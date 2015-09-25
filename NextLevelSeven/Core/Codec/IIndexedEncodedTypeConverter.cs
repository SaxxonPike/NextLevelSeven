namespace NextLevelSeven.Core.Codec
{
    /// <summary>Get or set elements via an HL7 data value codec.</summary>
    /// <typeparam name="TDecoded"></typeparam>
    public interface IIndexedEncodedTypeConverter<TDecoded>
    {
        /// <summary>Get or set element data at the specified index.</summary>
        TDecoded this[int index]
        {
            get;
            set;
        }
    }
}