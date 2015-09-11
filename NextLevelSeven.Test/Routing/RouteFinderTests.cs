using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Routing;

namespace NextLevelSeven.Test.Routing
{
    [TestClass]
    public class RouteFinderTests : RoutingTestFixture
    {
        [TestMethod]
        public void RouteFinder_CanBeInitializedWithNoRoutes()
        {
            var routeFinder = new RouteFinder();
            Assert.AreEqual(0, routeFinder.Routers.Count,
                "There should be zero routes when initializing with no parameters.");
        }

        [TestMethod]
        public void RouteFinder_CanBeInitializedWithParamsRoutes()
        {
            var routeFinder = new RouteFinder(new NullRouter(true), new NullRouter(false));
            Assert.AreEqual(2, routeFinder.Routers.Count, "There should be two routes.");
        }

        [TestMethod]
        public void RouteFinder_CanBeInitializedWithEnumerableRoutes()
        {
            var routeFinder = new RouteFinder(new IRouter[] {new NullRouter(true), new NullRouter(false)}.AsEnumerable());
            Assert.AreEqual(2, routeFinder.Routers.Count, "There should be two routes.");
        }

        [TestMethod]
        public void RouteFinder_FindsSingleRoute_WithOneEligibleRouter()
        {
            var router1 = new NullRouter(true);
            var router2 = new NullRouter(false);
            var routeFinder = new RouteFinder(router1, router2);
            var message = Message.Create(ExampleMessages.Standard);
            var routeResult = routeFinder.Route(message);

            Assert.IsTrue(routeResult, "A valid route should have been found.");
            Assert.IsTrue(router1.Checked, "Route 1 should have been queried.");
            Assert.IsTrue(router1.Routed, "Route 1 should have been rerouted.");
            Assert.IsFalse(router2.Checked, "Route 2 should not have been queried.");
            Assert.IsFalse(router2.Routed, "Route 2 should not have been rerouted.");
        }

        [TestMethod]
        public void RouteFinder_FindsSingleRoute_WithMultipleEligibleRouters()
        {
            var router1 = new NullRouter(true);
            var router2 = new NullRouter(true);
            var routeFinder = new RouteFinder(router1, router2);
            var message = Message.Create(ExampleMessages.Standard);
            var routeResult = routeFinder.Route(message);

            Assert.IsTrue(routeResult, "A valid route should have been found.");
            Assert.IsTrue(router1.Checked, "Route 1 should have been queried.");
            Assert.IsTrue(router1.Routed, "Route 1 should have been rerouted.");
            Assert.IsFalse(router2.Checked, "Route 2 should not have been queried.");
            Assert.IsFalse(router2.Routed, "Route 2 should not have been rerouted.");
        }

        [TestMethod]
        public void RouteFinder_FindsSingleRoute_WithMultipleEligibleRouters_WhenFirstIsNotEligible()
        {
            var router1 = new NullRouter(false);
            var router2 = new NullRouter(true);
            var routeFinder = new RouteFinder(router1, router2);
            var message = Message.Create(ExampleMessages.Standard);
            var routeResult = routeFinder.Route(message);

            Assert.IsTrue(routeResult, "A valid route should have been found.");
            Assert.IsTrue(router1.Checked, "Route 1 should have been queried.");
            Assert.IsFalse(router1.Routed, "Route 1 should not have been routed.");
            Assert.IsTrue(router2.Checked, "Route 2 should have been queried.");
            Assert.IsTrue(router2.Routed, "Route 2 should have been rerouted.");
        }

        [TestMethod]
        public void RouteFinder_FindsNoRoutes_WithNoEligibleRouters()
        {
            var router1 = new NullRouter(false);
            var router2 = new NullRouter(false);
            var routeFinder = new RouteFinder(router1, router2);
            var message = Message.Create(ExampleMessages.Standard);
            var routeResult = routeFinder.Route(message);

            Assert.IsFalse(routeResult, "A valid route should not have been found.");
            Assert.IsTrue(router1.Checked, "Route 1 should have been queried.");
            Assert.IsFalse(router1.Routed, "Route 1 should not have been rerouted.");
            Assert.IsTrue(router2.Checked, "Route 2 should have been queried.");
            Assert.IsFalse(router2.Routed, "Route 2 should not have been rerouted.");
        }
    }
}