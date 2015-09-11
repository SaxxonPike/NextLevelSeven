using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;
using NextLevelSeven.Routing;

namespace NextLevelSeven.Test.Routing
{
    [TestClass]
    public class ListenerRouterTests : RoutingTestFixture
    {
        [TestMethod]
        public void ListenerRouter_ReceivesMessages()
        {
            var routed = false;
            var message = Message.Create(ExampleMessages.Standard);
            var listener = new ListenerRouter(m => routed = true, new NullRouter(true));
            Assert.IsFalse(routed, "Test initialized incorrectly.");
            message.RouteTo(listener);
            Assert.IsTrue(routed, "Listener action was not called.");
        }

        [TestMethod]
        public void ListenerRouter_PassesMessagesThrough()
        {
            var routed = false;
            var router = new NullRouter(true);
            var message = Message.Create(ExampleMessages.Standard);
            var listener = new ListenerRouter(m => routed = true, router);
            Assert.IsFalse(routed, "Test initialized incorrectly.");
            message.RouteTo(listener);
            Assert.IsTrue(router.Checked, "Listener did not pass the message on.");
        }

        [TestMethod]
        public void ListenerRouter_PassesCorrectData()
        {
            INativeMessage routedData = null;
            var message = Message.Create(ExampleMessages.Standard);
            var router = new ListenerRouter(m => { routedData = m; });
            Assert.IsNull(routedData, "Test initialized incorrectly.");
            message.RouteTo(router);
            Assert.IsNotNull(routedData);
            Assert.AreEqual(message.Value, routedData.Value);
        }

        [TestMethod]
        public void ListenerRouter_ReturnsSuccessIfNoTargetRouter()
        {
            var routed = false;
            var message = Message.Create(ExampleMessages.Standard);
            var listener = new ListenerRouter(m => routed = true);
            Assert.IsFalse(routed, "Test initialized incorrectly.");
            Assert.IsTrue(message.RouteTo(listener), "Listener must return True when there is no target router.");
        }
    }
}