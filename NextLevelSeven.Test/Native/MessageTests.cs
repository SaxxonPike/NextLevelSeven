using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class MessageTests
    {
        [TestInitialize]
        public void Message_Initialization()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.IsNotNull(message, "Message was not initialized.");
            Debug.WriteLine(message.Type);
        }

        [TestMethod]
        public void Message_ConvertsMshCorrectly()
        {
            var message = Message.Create(ExampleMessages.MshOnly);
            Assert.AreEqual(ExampleMessages.MshOnly, message.Value, "MSH conversion back to string did not match.");
        }

        [TestMethod]
        public void Message_ReturnsBasicMessage()
        {
            var message = Message.Create();
            Assert.AreEqual(1, message.DescendantCount, @"Default message should not contain multiple segments.");
            Assert.AreEqual("MSH", message[1].Type, @"Default message should create an MSH segment.");
            Assert.AreEqual(@"^~\&", message[1][2].Value,
                @"Default message should use standard HL7 encoding characters.");
            Assert.AreEqual("|", message[1][1].Value,
                @"Default message should use standard HL7 field delimiter character.");
        }

        [TestMethod]
        public void Message_ThrowsOnNullData()
        {
            It.Throws<MessageException>(() => Message.Create(null));
        }

        [TestMethod]
        public void Message_ThrowsOnEmptyData()
        {
            It.Throws<MessageException>(() => Message.Create(string.Empty));
        }

        [TestMethod]
        public void Message_ThrowsOnShortData()
        {
            It.Throws<MessageException>(() => Message.Create("MSH|123"));
        }

        [TestMethod]
        public void Message_CanRetrieveMessageTypeAndTriggerEvent()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.AreEqual("ADT", message.Type, "Message type is incorrect.");
            Assert.AreEqual("A17", message.TriggerEvent, "Message trigger event is incorrect.");
        }

        [TestMethod]
        public void Message_CanParseMessageDate()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.IsTrue(message.Time.HasValue, "Parsed message date is incorrect.");
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), message.Time.Value.DateTime);
        }

        [TestMethod]
        public void Message_CanRetrieveMessageVersion()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.AreEqual("2.3", message.Version, "Message version is incorrect.");
        }

        [TestMethod]
        public void Message_CanRetrievePatientId()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var pid = message["PID"].First();
            Assert.AreEqual("Colon", pid[5][0][1].Value, "Patient name is incorrect.");
        }

        [TestMethod]
        public void Message_CanRetrieveMultipleSegments()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.AreEqual(3, message["OBX"].Count(), "Incorrect number of segments were found.");
        }

        [TestMethod]
        public void Message_CanRetrieveRepetitions()
        {
            var message = Message.Create(ExampleMessages.RepeatingName);
            var pid = message["PID"].First();
            Assert.AreEqual("Lincoln^Abe~Bro~Dude", pid[5][0].Value,
                "Retrieving full field data using index 0 returned incorrect data.");
            Assert.AreEqual("Lincoln^Abe", pid[5][1].Value, "Retrieving first repetition returned incorrect data.");
            Assert.AreEqual("Bro", pid[5][2].Value, "Retrieving second repetition returned incorrect data.");
            Assert.AreEqual("Dude", pid[5][3].Value, "Retrieving third repetition returned incorrect data.");
        }

        [TestMethod]
        public void Message_RetrievalMethodsAreIdentical()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.AreEqual(message.GetField(1).Value, message[1].Value,
                "Retrieval methods differ at the segment level.");
            Assert.AreEqual(message.GetField(1, 2).Value, message[1][2].Value,
                "Retrieval methods differ at the field level.");
            Assert.AreEqual(message.GetField(1, 2, 0).Value, message[1][2][0].Value,
                "Retrieval methods differ at the repetition level.");
            Assert.AreEqual(message.GetField(1, 2, 0, 1).Value, message[1][2][0][1].Value,
                "Retrieval methods differ at the component level.");
        }

        [TestMethod]
        public void Message_HasUniqueDescendantKeys()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var keys = message.Segments.Select(s => s.Key).ToList();
            var distinctKeys = keys.Distinct();

            foreach (var key in keys)
            {
                Debug.WriteLine(key);
            }
            Assert.AreEqual(distinctKeys.Count(), message.Segments.Count(), "Segments don't have entirely unique keys.");
        }

        [TestMethod]
        public void Message_HasUniqueKeys()
        {
            var message1 = Message.Create(ExampleMessages.Standard);
            var message2 = Message.Create(ExampleMessages.Standard);
            Assert.AreNotEqual(message1.Key, message2.Key);
        }

        [TestMethod]
        public void Message_CanBeCloned()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var clone = message.Clone();
            Assert.AreNotSame(message, clone, "Cloned message is the same referenced object.");
            Assert.AreEqual(message.Value, clone.Value, "Cloned message has different contents.");
        }

        [TestMethod]
        public void Message_WithIdentivalValueToAnotherMessage_IsEquivalent()
        {
            var message1 = Message.Create(ExampleMessages.Standard);
            var message2 = Message.Create(ExampleMessages.Standard);
            Assert.AreEqual(message1, message2);
        }

        [TestMethod]
        public void Message_WhenCreatedUsingString_IsEquivalentToTheString()
        {
            var message = Message.Create(ExampleMessages.Standard);
            Assert.AreEqual(message, ExampleMessages.Standard);
        }

        [TestMethod]
        public void Message_WithOnlyOneSegment_WillClaimToHaveSignificantDescendants()
        {
            var message = Message.Create();
            Assert.IsTrue(message.HasSignificantDescendants,
                @"Message should claim to have significant descendants if any segments do.");
        }

        [TestMethod]
        public void Message_UsesReasonableMemory_WhenParsingLargeMessages()
        {
            var before = GC.GetTotalMemory(true);
            var message = Message.Create();
            message[1000000][1000000].Value = Randomized.String();
            var messageString = message.Value;
            var usage = GC.GetTotalMemory(false) - before;
            var overhead = usage - (messageString.Length << 1);
            var usePerCharacter = (overhead/(messageString.Length << 1));
            Assert.IsTrue(usePerCharacter < 20);
        }

        [TestMethod]
        public void Message_CanMapSegments()
        {
            var id = Randomized.String();
            IMessage tree = Message.Create(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id), tree.GetValue(1));
        }

        [TestMethod]
        public void Message_CanMapFields()
        {
            var id = Randomized.String();
            IMessage tree = Message.Create(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(id, tree.GetValue(1, 3));
        }

        [TestMethod]
        public void Message_CanMapRepetitions()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Create(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 2));
        }

        [TestMethod]
        public void Message_CanMapComponents()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Create(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 2));
        }

        [TestMethod]
        public void Message_CanMapSubcomponents()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Create(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 1, 2));
        }
    }
}