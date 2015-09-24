using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Core
{
    static public class EncodingExtensions
    {
        static public string Escape(this IReadOnlyEncoding encoding, string data)
        {
            return EncodingOperations.Escape(encoding, data);
        }

        static public string UnEscape(this IReadOnlyEncoding encoding, string data)
        {
            return EncodingOperations.UnEscape(encoding, data);
        }
    }
}
