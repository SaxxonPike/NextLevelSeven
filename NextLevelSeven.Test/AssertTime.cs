using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    static public class AssertTime
    {
        static public void IsWithin(long tolerance, long measured)
        {
            if (measured > tolerance)
            {
                Assert.Inconclusive("Test was slow. Measured: {0}ms. Tolerance: {1}ms.", measured, tolerance);
            }
        }
    }
}
