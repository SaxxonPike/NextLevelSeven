using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NextLevelSeven.Test.Testing
{
    [ExcludeFromCodeCoverage]
    public static class Any
    {
        private static readonly Random Rng = new Random();

        public static string Date()
        {
            return string.Concat(
                PaddedNumber(1900, 2015, 4),
                PaddedNumber(01, 12, 2),
                PaddedNumber(01, 28, 2)
                );
        }

        public static string DateTime()
        {
            return string.Concat(
                Date(),
                PaddedNumber(00, 24, 2),
                PaddedNumber(00, 60, 2),
                PaddedNumber(00, 60, 2)
                );
        }

        public static string DateTimeMilliseconds()
        {
            return string.Concat(
                DateTime(),
                ".",
                Number(10000).ToString()
                );
        }

        public static string DateWithTimeZone()
        {
            return string.Concat(
                Date(),
                TimeZone()
                );
        }

        public static string DateTimeWithTimeZone()
        {
            return string.Concat(
                DateTime(),
                TimeZone()
                );
        }

        public static string DateTimeMillisecondsWithTimeZone()
        {
            return string.Concat(
                DateTimeMilliseconds(),
                TimeZone()
                );
        }

        public static string TimeZone()
        {
            return string.Concat(
                Number(2) == 0 ? "+" : "-",
                PaddedNumber(00, 12, 2),
                PaddedNumber(00, 60, 2)
                );
        }

        public static string Decimal()
        {
            return Rng.NextDouble().ToString(CultureInfo.InvariantCulture);
        }

        public static string DelimitedString(string delimiter, int count = 5)
        {
            return string.Join(delimiter, StringSequence(count));
        }

        public static string PaddedNumber(int minInclusive, int maxExclusive, int padLength)
        {
            var result = Number(minInclusive, maxExclusive).ToString();
            return string.Concat(new string('0', padLength - result.Length), result);
        }

        public static string Message()
        {
            return string.Join("\r", string.Join("|",
                ExampleMessageRepository.Minimum,
                String(), String(), // receiver
                String(), String(), // sender
                DateTime(), // message date/time
                String(), // security
                string.Join("^", StringCaps(3), StringCaps(3)), // message type/event
                String(), // control id
                String(3), // processing id
                string.Join(".", "2", Number(1, 7).ToString(CultureInfo.InvariantCulture)) // version
                ), Segment(), Segment(), Segment());
        }

        public static string Segment()
        {
            return string.Join("|", StringCaps(3), String(), String());
        }

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

        public static IEnumerable<string> StringSequence(int count)
        {
            while (count > 0)
            {
                yield return StringLetters();
                count--;
            }
        }

        public static string StringCaps()
        {
            return String().ToUpperInvariant().Replace('-', 'A');
        }

        public static string StringCaps(int length)
        {
            return String(length).ToUpperInvariant().Replace('-', 'A');
        }

        public static string StringLetters(int length = 16)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append((char) Number(0x41, 0x41 + 26));
            }
            return builder.ToString();
        }

        public static string StringNumbers(int length = 16)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append((char) Number(0x30, 0x30 + 10));
            }
            return builder.ToString();
        }

        public static string Symbol()
        {
            // exclude minus character because String() generates it
            return new string(OneOf("|!@#$%^&*()=_+[]{};':\"/?,.<>`~".ToCharArray()), 1);
        }

        public static T OneOf<T>(IEnumerable<T> values)
        {
            var valueArray = values.ToArray();
            return valueArray[Rng.Next(valueArray.Length)];
        }

    }
}