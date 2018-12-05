using System;

namespace NextLevelSeven.Core.Encoding
{
    public static class EncodingExtensions
    {
        public static string Escape(this IReadOnlyEncoding encoding, string data)
        {
            return data == null 
                ? null 
                : EncodingOperations.Escape(encoding, data.AsSpan());
        }

        public static string UnEscape(this IReadOnlyEncoding encoding, string data)
        {
            return EncodingOperations.UnEscape(encoding, data);
        }
    }
}