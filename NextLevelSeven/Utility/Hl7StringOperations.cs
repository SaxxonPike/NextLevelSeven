using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    public static class Hl7StringOperations
    {
        public static string NormalizeLineEndings(string s, char delimiter)
        {
            return s == null
                ? null
                : s.Replace(Environment.NewLine, new string(delimiter, 1))
                .Replace('\n', delimiter);
        }
    }
}
