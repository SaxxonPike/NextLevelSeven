using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Generators;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.MessageGeneration
{
    [TestClass]
    public class AckMessageGeneratorTests
    {
        private string _controlId;
        private INativeMessage _message;
        private string _trigger;
        private string _type;

        [TestInitialize]
        public void AckMessageGenerator_Initialize()
        {
            _type = Randomized.StringCaps(3);
            _trigger = Randomized.StringCaps(3);
            _controlId = Randomized.String();
            _message = MessageGenerator.Generate(_type, _trigger, _controlId);            
        }

        [TestMethod]
        public void AckMessageGenerator_GeneratesAcceptCode()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual("AA", ack[2][1].Value);
        }

        [TestMethod]
        public void AckMessageGenerator_GeneratesErrorCode()
        {
            var ack = AckMessageGenerator.GenerateError(_message);
            Assert.AreEqual("AE", ack[2][1].Value);
        }

        [TestMethod]
        public void AckMessageGenerator_GeneratesRejectCode()
        {
            var ack = AckMessageGenerator.GenerateReject(_message);
            Assert.AreEqual("AR", ack[2][1].Value);
        }

        [TestMethod]
        public void AckMessageGenerator_UsesAckType()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual("ACK", ack.Type);
        }

        [TestMethod]
        public void AckMessageGenerator_StartsWithMsh()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual("MSH", ack[1][0].Value);
        }

        [TestMethod]
        public void AckMessageGenerator_ContainsMsa()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.IsNotNull(ack["MSA"].FirstOrDefault());
        }

        [TestMethod]
        public void AckMessageGenerator_MatchesSenderApplication()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual(_message.Sender.Application, ack.Receiver.Application);
        }

        [TestMethod]
        public void AckMessageGenerator_MatchesSenderFacility()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual(_message.Sender.Facility, ack.Receiver.Facility);
        }

        [TestMethod]
        public void AckMessageGenerator_MatchesControlId()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual(_controlId, ack["MSA"].First()[2].Value);
        }
    }
}
