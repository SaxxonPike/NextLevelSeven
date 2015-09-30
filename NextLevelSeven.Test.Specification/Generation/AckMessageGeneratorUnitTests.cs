﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Specification.Generation;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Specification.Generation
{
    [TestClass]
    public class AckMessageGeneratorUnitTests : GenerationTestFixture
    {
        private string _controlId;
        private IMessage _message;
        private string _trigger;
        private string _type;

        [TestInitialize]
        public void AckMessageGenerator_Initialize()
        {
            _type = Mock.StringCaps(3);
            _trigger = Mock.StringCaps(3);
            _controlId = Mock.String();
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
        public void AckMessageGenerator_GeneratesErrorCodeWithReason()
        {
            var reason = Mock.String();
            var ack = AckMessageGenerator.GenerateError(_message, reason);
            Assert.AreEqual(reason, ack[2][3].Value);
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
            Assert.AreEqual("ACK", ack.Details.Type);
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
            Assert.IsNotNull(ack.Segments.OfType("MSA").FirstOrDefault());
        }

        [TestMethod]
        public void AckMessageGenerator_MatchesSenderApplication()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual(_message.Details.Sender.Application, ack.Details.Receiver.Application);
        }

        [TestMethod]
        public void AckMessageGenerator_MatchesSenderFacility()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual(_message.Details.Sender.Facility, ack.Details.Receiver.Facility);
        }

        [TestMethod]
        public void AckMessageGenerator_MatchesControlId()
        {
            var ack = AckMessageGenerator.GenerateSuccess(_message);
            Assert.AreEqual(_controlId, ack.Segments.OfType("MSA").First()[2].Value);
        }
    }
}