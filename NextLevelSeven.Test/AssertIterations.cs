using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    public static class AssertIterations
    {
        public static void AtLeast(long threshold, long observed)
        {
            if (observed < threshold)
            {
                Assert.Inconclusive("Test was slow. Measured: {0} iterations. Threshold: {1} iterations.", observed,
                    threshold);
            }
        }
    }
}