using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    static public class AssertIterations
    {
        static public void AtLeast(long threshold, long observed)
        {
            if (observed < threshold)
            {
                Assert.Inconclusive("Test was slow. Measured: {0} iterations. Threshold: {1} iterations.", observed, threshold);
            }
        }
    }
}
