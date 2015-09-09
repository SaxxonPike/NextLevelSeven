using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Routing;

namespace NextLevelSeven.Test.Routing
{
    [TestClass]
    public class BroadcastRouterTests
    {
        [TestMethod]
        public void BroadcastRouter_CanBeInitializedWithoutRoutes()
        {
            var router = new BroadcastRouter();
            Assert.AreEqual(0, router.Routers.Count, "There should be no routers.");
        }

        [TestMethod]
        public void BroadcastRouter_CanBeInitializedWithRoutes()
        {
            var router = new BroadcastRouter(new NullRouter(true), new NullRouter(false));
            Assert.AreEqual(2, router.Routers.Count, "There should be two routers.");
        }

        [TestMethod]
        public void BroadcastRouter_CanBeInitializedWithRoutesAsEnumerable()
        {
            var router = new BroadcastRouter(new IRouter[] { new NullRouter(true), new NullRouter(false) }.AsEnumerable());
            Assert.AreEqual(2, router.Routers.Count, "There should be two routers.");
        }

        [TestMethod]
        public void BroadcastRouter_SendsMessages()
        {
            var subRouter1 = new NullRouter(true);
            var subRouter2 = new NullRouter(true);
            var router = new BroadcastRouter(subRouter1, subRouter2);
            router.Route(null);
            Assert.IsTrue(subRouter1.Checked, "Subrouter 1 should have been checked.");
            Assert.IsTrue(subRouter2.Checked, "Subrouter 2 should have been checked.");
        }

        [TestMethod]
        public void BroadcastRouter_IsSuccessfulWhenAtLeastOneRouteIsSuccessful()
        {
            var subRouter1 = new NullRouter(false);
            var subRouter2 = new NullRouter(true);
            var router = new BroadcastRouter(subRouter1, subRouter2);
            Assert.IsTrue(router.Route(null));
        }

        [TestMethod]
        public void BroadcastRouter_IsNotSuccessfulWhenNoRoutesAreSuccessful()
        {
            var subRouter1 = new NullRouter(false);
            var subRouter2 = new NullRouter(false);
            var router = new BroadcastRouter(subRouter1, subRouter2);
            Assert.IsFalse(router.Route(null));
        }
    }
}
