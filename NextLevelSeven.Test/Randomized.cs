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
    }
}
