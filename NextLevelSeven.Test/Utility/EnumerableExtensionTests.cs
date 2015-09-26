using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Utility
{
    [TestClass]
    public class EnumerableExtensionTests : UtilityTestFixture
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
