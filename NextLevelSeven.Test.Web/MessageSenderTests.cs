using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Web
{
    [TestClass]
    public class MessageSenderTests : WebTestFixture
    {
        [TestMethod]
        public void MessageSender_CanSendMessages()
        {
            var received = false;
            MessageSenderMock.SendData(ExampleMessages.Standard, receivedHandler: (sender, e) => { received = true; });
            Assert.IsTrue(received, "Received event was not raised.");
        }
    }
}