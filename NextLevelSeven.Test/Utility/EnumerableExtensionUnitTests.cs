using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Testing;

namespace NextLevelSeven.Test.Utility
{
    [TestClass]
    public class EnumerableExtensionUnitTests : UtilityTestFixture
    {
        [TestMethod]
        public void EnumerableExtension_YieldsItem()
        {
            var item = new object();
            var yielded = UtilityMocks.Yield(item).ToList();
            Assert.AreEqual(1, yielded.Count);
            Assert.AreSame(item, yielded.First());
        }
    }
}