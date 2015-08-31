using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.MessageGeneration;

namespace NextLevelSeven.Test.MessageGeneration
{
    [TestClass]
    public class MessageGeneratorTests
    {
        [TestMethod]
        public void MessageGenerator_GeneratesProperlyFormattedMessage()
        {
            var message = MessageGenerator.Generate("ADT", "A01", Randomized.String());
            Debug.WriteLine(message);
            Assert.AreEqual("|", message[1][1].Value, @"MSH-1 is not the pipe character.");
            Assert.AreEqual("^~\\&", message[1][2].Value, @"MSH-2 is not the standard set of encoding characters.");
            Assert.IsNotNull(message.ProcessingId, @"Processing ID is null.");
            Assert.IsNotNull(message.Type, @"Message type is null.");
            Assert.IsNotNull(message.TriggerEvent, @"Message trigger event is null.");
            Assert.IsNotNull(message.Version, @"Version number is null.");
        }
    }
}
