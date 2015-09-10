using System;
using System.Text;

namespace NextLevelSeven.Test
{
    public static class Randomized
    {
        private static readonly Random Rng = new Random();

        public static int Number()
        {
            return Rng.Next(int.MaxValue);
        }

        public static int Number(int maxExclusiveValue)
        {
            return Rng.Next(maxExclusiveValue);
        }

        public static int Number(int minInclusiveValue, int maxExclusiveValue)
        {
            return Rng.Next(minInclusiveValue, maxExclusiveValue);
        }

        public static string String()
        {
            return Guid.NewGuid().ToString();
        }

        public static string String(int length)
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

        public static string StringCaps()
        {
            return String().ToUpperInvariant().Replace('-', 'A');
        }

        public static string StringCaps(int length)
        {
            return String(length).ToUpperInvariant().Replace('-', 'A');
        }

        public static string StringLetters()
        {
            return StringLetters(16);
        }

        public static string StringLetters(int length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append((char) Number(0x41, 0x41 + 26));
            }
            return builder.ToString();
        }

        public static string StringNumbers()
        {
            return StringNumbers(16);
        }

        public static string StringNumbers(int length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append((char) Number(0x30, 0x30 + 10));
            }
            return builder.ToString();
        }
    }
}