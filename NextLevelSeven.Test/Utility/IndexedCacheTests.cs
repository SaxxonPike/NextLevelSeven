using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Test.Utility
{
    [TestClass]
    public class IndexedCacheTests : UtilityTestFixture
    {
        static private IIndexedCache<int, string> InitializeCache(Func<int, string> factory)
        {
            return UtilityMocks.GetIndexedCache(factory);
        }

        [TestMethod]
        public void IndexedCache_StartsEmpty()
        {
            var cache = InitializeCache(i => string.Empty);
            Assert.AreEqual(0, cache.Count);
        }

        [TestMethod]
        public void IndexedCache_RunsFactoryWhenNeeded()
        {
            var factoryHits = 0;
            var cache = InitializeCache(i => {
                factoryHits++;
                return factoryHits.ToString();
            });
            Assert.IsNotNull(cache[0]);
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, factoryHits);
        }

        [TestMethod]
        public void IndexedCache_DoesNotRunFactoryMultipleTimesForAnIndex()
        {
            var factoryHits = 0;
            var cache = InitializeCache(i =>
            {
                factoryHits++;
                return factoryHits.ToString();
            });
            Assert.AreEqual(cache[0], "1");
            cache[0] = Randomized.String();
            cache[1] = Randomized.String();
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(1, factoryHits);
        }

        [TestMethod]
        public void IndexedCache_Clears()
        {
            var cache = InitializeCache(i => string.Empty);
            cache[0] = Randomized.String();
            Assert.AreEqual(1, cache.Count);
            cache.Clear();
            Assert.AreEqual(0, cache.Count);
        }

        [TestMethod]
        public void IndexedCache_RetainsValues()
        {
            var value1 = Randomized.String();
            var value2 = Randomized.String();
            var cache = InitializeCache(i => string.Empty);
            Assert.AreEqual(cache[0], string.Empty);
            cache[1] = value1;
            cache[2] = value2;
            Assert.AreEqual(cache[1], value1);
            Assert.AreEqual(cache[2], value2);
        }

        [TestMethod]
        public void IndexedCache_RemovesValue()
        {
            var value0 = Randomized.String();
            var value1 = Randomized.String();
            var value2 = Randomized.String();
            var cache = InitializeCache(i => string.Empty);
            cache[0] = value0;
            cache[1] = value1;
            cache[2] = value2;
            Assert.IsTrue(cache.Remove(1));
            Assert.AreEqual(cache[1], string.Empty);
        }

        [TestMethod]
        public void IndexedCache_DoesNotRemoveNonExistantValue()
        {
            var value0 = Randomized.String();
            var value1 = Randomized.String();
            var value2 = Randomized.String();
            var cache = InitializeCache(i => string.Empty);
            cache[0] = value0;
            cache[1] = value1;
            cache[2] = value2;
            Assert.IsFalse(cache.Remove(3));
            Assert.AreEqual(cache.Count, 3);
        }

    }
}
