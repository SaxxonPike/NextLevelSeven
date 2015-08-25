using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class MessageFixture
    {

        [TestInitialize]
        public void Message_Initialization()
        {
            var message = new Message(ExampleMessages.Standard);
            Assert.IsNotNull(message, "Message was not initialized.");
            Debug.WriteLine(message.Type);
        }

        [TestMethod]
        public void Message_CanInitializeEmptyMessage()
        {
            var message = new Message(string.Empty);
            Assert.IsNotNull(message, "Could not initialize empty message.");
        }

        [TestMethod]
        public void Message_CanInitializeNullMessage()
        {
            var message = new Message(null);
            Assert.IsNotNull(message, "Could not initialize null message.");
        }

        [TestMethod]
        public void Message_CanRetrieveMessageTypeAndTriggerEvent()
        {
            var message = new Message(ExampleMessages.Standard);
            Assert.AreEqual("ADT", message.Type, "Message type is incorrect.");
            Assert.AreEqual("A17", message.TriggerEvent, "Message trigger event is incorrect.");
        }

        [TestMethod]
        public void Message_CanParseMessageDate()
        {
            var message = new Message(ExampleMessages.Standard);
            Assert.IsTrue(message.Time.HasValue, "Parsed message date is incorrect.");
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), message.Time.Value.DateTime);
        }

        [TestMethod]
        public void Message_CanRetrieveMessageVersion()
        {
            var message = new Message(ExampleMessages.Standard);
            Assert.AreEqual("2.3", message.Version, "Message version is incorrect.");
        }

        [TestMethod]
        public void Message_CanRetrievePatientId()
        {
            var message = new Message(ExampleMessages.Standard);
            var pid = message["PID"].First();
            Assert.AreEqual("Colon", pid[5][0][1].Value, "Patient name is incorrect.");
        }

        [TestMethod]
        public void Message_CanRetrieveMultipleSegments()
        {
            var message = new Message(ExampleMessages.Standard);
            Assert.AreEqual(3, message["OBX"].Count(), "Incorrect number of segments were found.");
        }

        [TestMethod]
        public void Message_CanRetrieveRepetitions()
        {
            var message = new Message(ExampleMessages.RepeatingName);
            var pid = message["PID"].First();
            Assert.AreEqual("Lincoln^Abe~Bro~Dude", pid[5][0].Value, "Retrieving full field data using index 0 returned incorrect data.");
            Assert.AreEqual("Lincoln^Abe", pid[5][1].Value, "Retrieving first repetition returned incorrect data.");
            Assert.AreEqual("Bro", pid[5][2].Value, "Retrieving second repetition returned incorrect data.");
            Assert.AreEqual("Dude", pid[5][3].Value, "Retrieving third repetition returned incorrect data.");
        }

        [TestMethod]
        public void Message_RetrievalMethodsAreIdentical()
        {
            var message = new Message(ExampleMessages.Standard);
            Assert.AreEqual(message.GetField(1).Value, message[1].Value, "Retrieval methods differ at the segment level.");
            Assert.AreEqual(message.GetField(1, 2).Value, message[1][2].Value, "Retrieval methods differ at the field level.");
            Assert.AreEqual(message.GetField(1, 2, 0).Value, message[1][2][0].Value, "Retrieval methods differ at the repetition level.");
            Assert.AreEqual(message.GetField(1, 2, 0, 1).Value, message[1][2][0][1].Value, "Retrieval methods differ at the component level.");
        }

        [TestMethod]
        public void Message_HasAllUniqueKeys()
        {
            var message = new Message(ExampleMessages.Standard);
            var keys = message.Segments.Select(s => s.Key);
            var distinctKeys = keys.Distinct();

            foreach (var key in keys)
            {
                Debug.WriteLine(key);
            }
            Assert.AreEqual(distinctKeys.Count(), message.Segments.Count(), "Segments don't have entirely unique keys.");
        }
    }
}
