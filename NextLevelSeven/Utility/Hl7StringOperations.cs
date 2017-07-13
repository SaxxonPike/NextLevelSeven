using System;

namespace NextLevelSeven.Utility
{
    public static class Hl7StringOperations
    {
        public static string NormalizeLineEndings(string s, char delimiter)
        {
            return s?
                .Replace(Environment.NewLine, new string(delimiter, 1))
                .Replace('\n', delimiter);
        }
    }
}
