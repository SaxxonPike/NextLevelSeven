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
            var request = Message.Create(ExampleMessages.Standard);
            request.Details.Sender.Application = Randomized.String();
            request.Details.Sender.Facility = Randomized.String();
            request.Details.Receiver.Application = null;
            request.Details.Receiver.Facility = null;

            var responseData = MessageSenderMock.SendMessage(request);
            Assert.IsNotNull(responseData, @"Response data was null.");
            var response = Message.Create(responseData);
            var responseTime = response.Details.Time;

            Assert.AreEqual(2, response.ValueCount, @"ACK must consist of exactly two segments.");
            Assert.AreEqual(request.Details.Sender.Application, response.Details.Receiver.Application,
                @"Application field doesn't match what was sent.");
            Assert.AreEqual(request.Details.Sender.Facility, response.Details.Receiver.Facility,
                @"Facility field doesn't match what was sent.");
            Assert.AreEqual(request.Details.ControlId, response["MSA"].First()[2].Value,
                @"MSA-2 doesn't match what was sent in MSH-10.");
            Assert.IsTrue(responseTime.HasValue, @"Message date cannot be null.");
            Assert.AreEqual(responseTime.Value.Date, DateTime.Now.Date, @"Message date of response isn't today.");
            Assert.AreEqual("ACK", response.Details.Type, "MSH-9-1 should be ACK.");
        }

        [TestMethod]
        public void MessageReceiver_DuringAcknowledgement_WithBadMessage_ReturnsProperAckStructure()
        {
            var responseData = MessageSenderMock.SendData("BadMessage");
            var response = Message.Create(responseData);

            Assert.AreEqual(2, response.ValueCount, @"ACK must consist of exactly two segments.");
            Assert.AreEqual("AR", response["MSA"].First()[1].Value, @"MSA-1 must be 'AR' when rejecting bad messages.");
            Assert.AreEqual("ACK", response.Details.Type, "MSH-9-1 should be ACK.");
        }
    }
}