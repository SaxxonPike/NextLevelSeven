using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class MessageParserUnitTests : ParsingTestFixture
    {
        [TestMethod]
        public void Message_Validates()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            Assert.IsTrue(message.Validate());
        }

        [TestMethod]
        public void Message_DeletesOutOfRangeIndex()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message.DeleteDescendant(2);
        }

        [TestMethod]
        public void Message_ThrowsOnInsertingNegativeIndex()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            AssertAction.Throws<ElementException>(() => message.InsertDescendant(Mock.String(), -2));
        }

        [TestMethod]
        public void Message_ThrowsOnDeletingNegativeIndex()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            AssertAction.Throws<ElementException>(() => message.DeleteDescendant(-2));
        }

        [TestMethod]
        public void Message_DeletesZeroLengthItem()
        {
            var message = Message.Parse(ExampleMessages.Minimum + "\r\r");
            message.DeleteDescendant(2);
        }

        [TestMethod]
        public void Message_ConvertsToBuilder()
        {
            var builder = Message.Parse(ExampleMessages.Standard);
            var beforeMessageString = builder.Value;
            var message = builder.ToBuilder();
            Assert.AreEqual(beforeMessageString, message.Value, "Conversion from parser to builder failed.");
        }

        [TestMethod]
        public void Message_ConvertsFromBuilder()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var beforeBuilderString = message.Value;
            var afterBuilder = Message.Parse(message);
            Assert.AreEqual(beforeBuilderString, afterBuilder.Value, "Conversion from builder to parser failed.");
        }

        [TestMethod]
        public void Message_ThrowsWithIncorrectFirstSegment()
        {
            AssertAction.Throws<ElementException>(() => Message.Parse(Mock.String()));
        }

        [TestMethod]
        public void Message_CanGetValues()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            Assert.AreEqual(ExampleMessages.Minimum, message.Values.First());
        }

        [TestMethod]
        public void Message_CanSetValues()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message.Values = new[] {ExampleMessages.Minimum, ExampleMessages.Minimum, ExampleMessages.Minimum};
            Assert.AreEqual(3, message.ValueCount);
        }

        [TestMethod]
        public void Message_CanIndexPastEnd()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            Assert.IsNull(message[5].Value);
        }

        [TestMethod]
        public void Message_CanMoveSegments()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var newMessage = message.Clone();
            newMessage[2].Move(3);
            Assert.AreEqual(message[2].Value, newMessage[3].Value);
        }

        [TestMethod]
        public void Message_Throws_WhenIndexedBelowOne()
        {
            var element = Message.Parse(ExampleMessages.Standard);
            string value = null;
            AssertAction.Throws<ParserException>(() => { value = element[0].Value; });
            Assert.IsNull(value);
        }

        [TestMethod]
        public void Message_CanSetMsh1()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message[1][1].Value = ":";
            Assert.AreEqual(':', message.Encoding.FieldDelimiter);
        }

        [TestMethod]
        public void Message_CanSetMsh2Component()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message[1][2].Value = "$~\\&";
            Assert.AreEqual('$', message.Encoding.ComponentDelimiter);
        }

        [TestMethod]
        public void Message_CanSetMsh2Escape()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message[1][2].Value = "^~$&";
            Assert.AreEqual('$', message.Encoding.EscapeCharacter);
        }

        [TestMethod]
        public void Message_CanSetMsh2Repetition()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message[1][2].Value = "^$\\&";
            Assert.AreEqual('$', message.Encoding.RepetitionDelimiter);
        }

        [TestMethod]
        public void Message_CanSetMsh2Subcomponent()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            message[1][2].Value = "^~\\$";
            Assert.AreEqual('$', message.Encoding.SubcomponentDelimiter);
        }

        [TestMethod]
        public void Message_CanSetMsh2Partially()
        {
            var parser = Message.Parse(ExampleMessages.Minimum + "|");
            parser[1][2].Value = "$";
            Assert.AreEqual("MSH|$|", parser.Value);
            Assert.AreEqual(parser.Encoding.EscapeCharacter, '\0');
            Assert.AreEqual(parser.Encoding.RepetitionDelimiter, '\0');
            Assert.AreEqual(parser.Encoding.SubcomponentDelimiter, '\0');
        }

        [TestMethod]
        public void Message_ConvertsMshCorrectly()
        {
            var message = Message.Parse(ExampleMessages.MshOnly);
            Assert.AreEqual(ExampleMessages.MshOnly, message.Value, "MSH conversion back to string did not match.");
        }

        [TestMethod]
        public void Message_ReturnsBasicMessage()
        {
            var message = Message.Parse();
            Assert.AreEqual(1, message.ValueCount, @"Default message should not contain multiple segments.");
            Assert.AreEqual("MSH", message[1].Type, @"Default message should create an MSH segment.");
            Assert.AreEqual(@"^~\&", message[1][2].Value,
                @"Default message should use standard HL7 encoding characters.");
            Assert.AreEqual("|", message[1][1].Value,
                @"Default message should use standard HL7 field delimiter character.");
        }

        [TestMethod]
        public void Message_ThrowsOnNullData()
        {
            AssertAction.Throws<ElementException>(() => Message.Parse((string) null));
        }

        [TestMethod]
        public void Message_ThrowsOnEmptyData()
        {
            AssertAction.Throws<ElementException>(() => Message.Parse(string.Empty));
        }

        [TestMethod]
        public void Message_ThrowsOnShortData()
        {
            AssertAction.Throws<ElementException>(() => Message.Parse("MSH|123"));
        }

        [TestMethod]
        public void Message_CanRetrieveMessageTypeAndTriggerEvent()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual("ADT", message.Details.Type, "Message type is incorrect.");
            Assert.AreEqual("A17", message.Details.TriggerEvent, "Message trigger event is incorrect.");
        }

        [TestMethod]
        public void Message_CanParseMessageDate()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.IsTrue(message.Details.Time.HasValue, "Parsed message date is incorrect.");
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), message.Details.Time.Value.DateTime);
        }

        [TestMethod]
        public void Message_CanRetrieveMessageVersion()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual("2.3", message.Details.Version, "Message version is incorrect.");
        }

        [TestMethod]
        public void Message_CanRetrievePatientId()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var pid = message.Segments.OfType("PID").First();
            Assert.AreEqual("Colon", pid[5][1][1].Value, "Patient name is incorrect.");
        }

        [TestMethod]
        public void Message_CanRetrieveMultipleSegments()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual(3, message.Segments.OfType("OBX").Count(), "Incorrect number of segments were found.");
        }

        [TestMethod]
        public void Message_CanRetrieveRepetitions()
        {
            var message = Message.Parse(ExampleMessages.RepeatingName);
            var pid = message.Segments.OfType("PID").First();
            Assert.AreEqual("Lincoln^Abe", pid[5][1].Value, "Retrieving first repetition returned incorrect data.");
            Assert.AreEqual("Bro", pid[5][2].Value, "Retrieving second repetition returned incorrect data.");
            Assert.AreEqual("Dude", pid[5][3].Value, "Retrieving third repetition returned incorrect data.");
        }

        [TestMethod]
        public void Message_RetrievalMethodsAreIdentical()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual(message.GetValue(1), message[1].Value,
                "Retrieval methods differ at the segment level.");
            Assert.AreEqual(message.GetValue(1, 3), message[1][3].Value,
                "Retrieval methods differ at the field level.");
            Assert.AreEqual(message.GetValue(1, 3, 1), message[1][3][1].Value,
                "Retrieval methods differ at the repetition level.");
            Assert.AreEqual(message.GetValue(1, 3, 1, 1), message[1][3][1][1].Value,
                "Retrieval methods differ at the component level.");
            Assert.AreEqual(message.GetValue(1, 3, 1, 1, 1), message[1][3][1][1][1].Value,
                "Retrieval methods differ at the component level.");
        }

        [TestMethod]
        public void Message_MultiRetrievalMethodsAreIdentical()
        {
            var message = Message.Parse(ExampleMessages.Variety);
            AssertArray.AreEqual(message.GetValues(1).ToArray(), message[1].Values.ToArray());
            AssertArray.AreEqual(message.GetValues(1, 3).ToArray(), message[1][3].Values.ToArray());
            AssertArray.AreEqual(message.GetValues(1, 3, 1).ToArray(), message[1][3][1].Values.ToArray());
            AssertArray.AreEqual(message.GetValues(1, 3, 1, 1).ToArray(), message[1][3][1][1].Values.ToArray());
            AssertArray.AreEqual(message.GetValues(1, 3, 1, 1, 1).ToArray(), message[1][3][1][1][1].Values.ToArray());
        }

        [TestMethod]
        public void Message_HasUniqueDescendantKeys()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var keys = message.Segments.Select(s => s.Key).ToList();
            var distinctKeys = keys.Distinct();

            foreach (var key in keys)
            {
                Debug.WriteLine(key);
            }
            Assert.AreEqual(distinctKeys.Count(), message.Segments.Count(), "Segments don't have entirely unique keys.");
        }

        [TestMethod]
        public void Message_CanBeCloned()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var clone = message.Clone();
            Assert.AreNotSame(message, clone, "Cloned message is the same referenced object.");
            Assert.AreEqual(message.Value, clone.Value, "Cloned message has different contents.");
        }

        [TestMethod]
        public void Message_WithIdentivalValueToAnotherMessage_IsEquivalent()
        {
            var message1 = Message.Parse(ExampleMessages.Standard);
            var message2 = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual(message1, message2);
        }

        [TestMethod]
        public void Message_WhenCreatedUsingString_IsEquivalentToTheString()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual(message, ExampleMessages.Standard);
        }

        [TestMethod]
        public void Message_WithOnlyOneSegment_WillClaimToHaveSignificantDescendants()
        {
            var message = Message.Parse();
            Assert.IsTrue(message.HasSignificantDescendants(),
                @"Message should claim to have significant descendants if any segments do.");
        }

        [TestMethod]
        public void Message_UsesReasonableMemory_WhenParsingLargeMessages()
        {
            var before = GC.GetTotalMemory(true);
            var message = Message.Parse();
            message[1000000][1000000].Value = Mock.String();
            var messageString = message.Value;
            var usage = GC.GetTotalMemory(false) - before;
            var overhead = usage - (messageString.Length << 1);
            var usePerCharacter = (overhead/(messageString.Length << 1));
            Assert.IsTrue(usePerCharacter < 20);
        }

        [TestMethod]
        public void Message_CanMapSegments()
        {
            var id = Mock.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id), tree.GetValue(1));
        }

        [TestMethod]
        public void Message_CanMapFields()
        {
            var id = Mock.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(id, tree.GetValue(1, 3));
        }

        [TestMethod]
        public void Message_CanMapRepetitions()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 2));
        }

        [TestMethod]
        public void Message_CanMapComponents()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 2));
        }

        [TestMethod]
        public void Message_CanMapSubcomponents()
        {
            var id1 = Mock.String();
            var id2 = Mock.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 1, 2));
        }

        [TestMethod]
        public void Message_CanAddDescendantsAtEnd()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var nextIndex = message.NextIndex;
            var count = message.ValueCount;
            var id = Mock.String();
            message[nextIndex].Value = id;
            Assert.AreEqual(count + 1, message.ValueCount,
                @"Number of elements after appending at the end of a message is incorrect.");
        }

        [TestMethod]
        public void Message_CanGetSegmentsByIndexer()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var segment = message[1];
            Assert.AreEqual(@"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|20130528073829||ADT^A17|14150278|P|2.3|",
                segment.Value);
        }

        [TestMethod]
        public void Message_CanDeleteSegment()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var segment1 = message[1].Value;
            var segment3 = message[3].Value;
            var segment4 = message[4].Value;
            message.Delete(2);
            Assert.AreEqual(segment1, message[1].Value, @"Expected message[1] to remain the same after delete.");
            Assert.AreEqual(segment3, message[2].Value, @"Expected message[3] to become message[2].");
            Assert.AreEqual(segment4, message[3].Value, @"Expected message[4] to become message[3].");
        }

        [TestMethod]
        public void Message_ValuesReturnsProperlySplitData()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var segmentStrings = message.Value.Split('\xD');
            var segments = message.Values.ToList();

            Assert.AreEqual(segmentStrings.Length, segments.Count,
                @"Splitting main value and calling Values returns different element counts.");

            for (var i = 0; i < segments.Count; i++)
            {
                Assert.AreEqual(segments[i], segmentStrings[i], @"Values are not equal.");
            }
        }
    }
}