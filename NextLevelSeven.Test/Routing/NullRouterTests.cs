using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.Routing
{
    [TestClass]
    public class NullRouterTests
    {
        [TestMethod]
        public void NullRouter_ReportsChecked_OnlyWhenAttempted()
        {
            var route = new NullRouter(false);
            Assert.IsFalse(route.Checked, "Route should not be checked initially.");
            route.Route(null);
            Assert.IsTrue(route.Checked, "Route should report checked.");
        }

        [TestMethod]
        public void NullRouter_ReportsRouted_WhenAttemptedAndSetTrue()
        {
            var route = new NullRouter(true);
            Assert.IsFalse(route.Routed, "Route should not be considered routed initially.");
            route.Route(null);
            Assert.IsTrue(route.Routed, "Route should be considered routed when initialized True.");
        }

        [TestMethod]
        public void NullRouter_ReportsRouted_WhenAttemptedAndSetFalse()
        {
            var route = new NullRouter(false);
            Assert.IsFalse(route.Routed, "Route should not be considered routed initially.");
            route.Route(null);
            Assert.IsFalse(route.Routed, "Route should not be considered routed when initialized False.");
        }

        [TestMethod]
        public void NullRouter_RetainsLastMessage()
        {
            var message = new NativeMessage(ExampleMessages.Standard);
            var router = new NullRouter(true);
            router.Route(message);
            Assert.AreEqual(message.ToString(), router.LastMessage.ToString(), "Message mismatch.");
        }
    }
}
