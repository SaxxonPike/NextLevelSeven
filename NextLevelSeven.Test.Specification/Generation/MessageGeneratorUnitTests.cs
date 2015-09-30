using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Specification.Generation;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Specification.Generation
{
    [TestClass]
    public class MessageGeneratorUnitTests : GenerationTestFixture
    {
        private string _controlId;
        private IMessage _message;
        private string _processingId;
        private string _receivingApplication;
        private string _receivingFacility;
        private string _sendingApplication;
        private string _sendingFacility;
        private string _trigger;
        private string _type;
        private string _version;

        [TestInitialize]
        public void MessageGenerator_GenerateMessage()
        {
            _controlId = Mock.String();
            _processingId = Mock.StringLetters(1);
            _receivingApplication = Mock.String();
            _receivingFacility = Mock.String();
            _sendingApplication = Mock.String();
            _sendingFacility = Mock.String();
            _trigger = Mock.StringCaps(3);
            _type = Mock.StringCaps(3);
            _version = string.Format("2.{0}", Mock.Number(1, 8));
            _message = MessageGenerator.Generate(
                _type,
                _trigger,
                _controlId,
                _processingId,
                _receivingApplication,
                _receivingFacility,
                _sendingApplication,
                _sendingFacility,
                _version);
        }

        [TestMethod]
        public void MessageGenerator_GeneratesProperEncodingCharacters()
        {
            Debug.WriteLine(_message);
            Assert.AreEqual("|", _message[1][1].Value, @"MSH-1 is not the pipe character.");
            Assert.AreEqual("^~\\&", _message[1][2].Value, @"MSH-2 is not the standard set of encoding characters.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesProcessingId()
        {
            Assert.AreEqual(_processingId, _message.Details.ProcessingId, @"Processing ID doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesReceivingApplication()
        {
            Assert.AreEqual(_receivingApplication, _message.Details.Receiver.Application,
                @"Receiving Application doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesReceivingFacility()
        {
            Assert.AreEqual(_receivingFacility, _message.Details.Receiver.Facility, @"Receiving Facility doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesSendingApplication()
        {
            Assert.AreEqual(_sendingApplication, _message.Details.Sender.Application,
                @"Sending Application doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesSendingFacility()
        {
            Assert.AreEqual(_sendingFacility, _message.Details.Sender.Facility, @"Sending Facility doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesVersion()
        {
            Assert.AreEqual(_version, _message.Details.Version, @"Version doesn't match.");
        }
    }
}