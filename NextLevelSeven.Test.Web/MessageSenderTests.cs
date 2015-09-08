using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Web;

namespace NextLevelSeven.Test.Web
{
    [TestClass]
    public class MessageSenderTests
    {
        [TestMethod]
        public void MessageSender_CanSendMessages()
        {
            var received = false;
            var response = MessageSenderMock.SendData(ExampleMessages.Standard, receivedHandler: (sender, e) => { received = true; });
            Assert.IsTrue(received, "Received event was not raised.");
        }
    }
}
