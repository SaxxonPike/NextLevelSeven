namespace NextLevelSeven.Core.Encoding
{
    public static class EncodingExtensions
    {
        public static string Escape(this IReadOnlyEncoding encoding, string data)
        {
            return EncodingOperations.Escape(encoding, data);
        }

        public static string UnEscape(this IReadOnlyEncoding encoding, string data)
        {
            return EncodingOperations.UnEscape(encoding, data);
        }
    }
}