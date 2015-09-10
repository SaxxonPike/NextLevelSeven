using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Generators;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.MessageGeneration
{
    [TestClass]
    public class MessageGeneratorTests
    {
        private INativeMessage _message;
        private string _controlId;
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
            _controlId = Randomized.String();
            _processingId = Randomized.StringLetters(1);
            _receivingApplication = Randomized.String();
            _receivingFacility = Randomized.String();
            _sendingApplication = Randomized.String();
            _sendingFacility = Randomized.String();
            _trigger = Randomized.StringCaps(3);
            _type = Randomized.StringCaps(3);
            _version = string.Format("2.{0}", Randomized.Number(1, 8));
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
            Assert.AreEqual(_processingId, _message.ProcessingId, @"Processing ID doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesReceivingApplication()
        {
            Assert.AreEqual(_receivingApplication, _message.Receiver.Application, @"Receiving Application doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesReceivingFacility()
        {
            Assert.AreEqual(_receivingFacility, _message.Receiver.Facility, @"Receiving Facility doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesSendingApplication()
        {
            Assert.AreEqual(_sendingApplication, _message.Sender.Application, @"Sending Application doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesSendingFacility()
        {
            Assert.AreEqual(_sendingFacility, _message.Sender.Facility, @"Sending Facility doesn't match.");
        }

        [TestMethod]
        public void MessageGenerator_GeneratesVersion()
        {
            Assert.AreEqual(_version, _message.Version, @"Version doesn't match.");
        }
    }
}
