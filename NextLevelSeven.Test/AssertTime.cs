using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    public static class AssertTime
    {
        public static void IsWithin(long tolerance, long measured)
        {
            if (measured > tolerance)
            {
                Assert.Inconclusive("Test was slow. Measured: {0}ms. Tolerance: {1}ms.", measured, tolerance);
            }
        }
    }
}