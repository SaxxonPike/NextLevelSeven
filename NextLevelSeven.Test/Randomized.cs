using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Test
{
    static public class Randomized
    {
        private static readonly Random Rng = new Random();

        static public int Number()
        {
            return Rng.Next(int.MaxValue);
        }

        static public int Number(int maxExclusiveValue)
        {
            return Rng.Next(maxExclusiveValue);
        }

        static public int Number(int minInclusiveValue, int maxExclusiveValue)
        {
            return Rng.Next(minInclusiveValue, maxExclusiveValue);
        }

        static public string String()
        {
            return Guid.NewGuid().ToString();
        }

        static public string String(int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            while (builder.Length < length)
            {
                builder.Append(Guid.NewGuid());
            }
            return builder.ToString().Substring(0, length);
        }

        static public string StringCaps()
        {
            return String().ToUpperInvariant().Replace('-', 'A');
        }

        static public string StringCaps(int length)
        {
            return String(length).ToUpperInvariant().Replace('-', 'A');
        }

        static public string StringLetters()
        {
            return StringLetters(16);
        }

        static public string StringLetters(int length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append((char)Number(0x41, 0x41 + 26));
            }
            return builder.ToString();
        }

        static public string StringNumbers()
        {
            return StringNumbers(16);
        }

        static public string StringNumbers(int length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append((char)Number(0x30, 0x30 + 10));
            }
            return builder.ToString();
        }
    }
}
