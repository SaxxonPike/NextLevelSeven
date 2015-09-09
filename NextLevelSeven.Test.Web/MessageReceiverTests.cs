using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Web
{
    [TestClass]
    public class MessageReceiverTests
    {
        [TestMethod]
        public void MessageReceiver_DuringAcknowledgement_ReturnsProperAckStructure()
        {
            var request = new Message(ExampleMessages.Standard);
            request.Sender.Application = Randomized.String();
            request.Sender.Facility = Randomized.String();
            request.Receiver.Application = null;
            request.Receiver.Facility = null;

            var responseData = MessageSenderMock.SendMessage(request);
            Assert.IsNotNull(responseData, @"Response data was null.");
            var response = new Message(responseData);
            var responseTime = response.Time;

            Assert.AreEqual(2, response.DescendantCount, @"ACK must consist of exactly two segments.");
            Assert.AreEqual(request.Sender.Application, response.Receiver.Application, @"Application field doesn't match what was sent.");
            Assert.AreEqual(request.Sender.Facility, response.Receiver.Facility, @"Facility field doesn't match what was sent.");
            Assert.AreEqual(request.ControlId, response["MSA"].First()[2].Value, @"MSA-2 doesn't match what was sent in MSH-10.");
            Assert.IsTrue(responseTime.HasValue, @"Message date cannot be null.");
            Assert.AreEqual(responseTime.Value.Date, DateTime.Now.Date, @"Message date of response isn't today.");
            Assert.AreEqual("ACK", response.Type, "MSH-9-1 should be ACK.");
        }

        [TestMethod]
        public void MessageReceiver_DuringAcknowledgement_WithBadMessage_ReturnsProperAckStructure()
        {
            var responseData = MessageSenderMock.SendData("BadMessage");
            var response = new Message(responseData);

            Assert.AreEqual(2, response.DescendantCount, @"ACK must consist of exactly two segments.");
            Assert.AreEqual("AR", response["MSA"].First()[1].Value, @"MSA-1 must be 'AR' when rejecting bad messages.");
            Assert.AreEqual("ACK", response.Type, "MSH-9-1 should be ACK.");
        }
    }
}
