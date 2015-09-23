using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class MessageParserTests : ParsingTestFixture
    {
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
            It.Throws<ElementException>(() => Message.Parse((string) null));
        }

        [TestMethod]
        public void Message_ThrowsOnEmptyData()
        {
            It.Throws<ElementException>(() => Message.Parse(string.Empty));
        }

        [TestMethod]
        public void Message_ThrowsOnShortData()
        {
            It.Throws<ElementException>(() => Message.Parse("MSH|123"));
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
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id), tree.GetValue(1));
        }

        [TestMethod]
        public void Message_CanMapFields()
        {
            var id = Randomized.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(id, tree.GetValue(1, 3));
        }

        [TestMethod]
        public void Message_CanMapRepetitions()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 2));
        }

        [TestMethod]
        public void Message_CanMapComponents()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 2));
        }

        [TestMethod]
        public void Message_CanMapSubcomponents()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Parse(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 1, 2));
        }

        [TestMethod]
        public void Message_CanAddDescendantsAtEnd()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var count = message.ValueCount;
            var id = Randomized.String();
            message[count + 1].Value = id;
            Assert.AreEqual(count + 1, message.ValueCount,
                @"Number of elements after appending at the end of a message is incorrect.");
        }

        [TestMethod]
        public void Message_Timely_AddsHighIndexSegment()
        {
            var testString = Randomized.String();
            var message = Message.Parse();
            var time = Measure.ExecutionTime(() => { message[HighIndex].Value = testString; });
            Assert.AreEqual(testString, message[HighIndex].Value);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Message_Timely_AddsLowIndexSegmentAndHighIndexField()
        {
            var testString = Randomized.String();
            var message = Message.Parse();
            var time = Measure.ExecutionTime(() => { message[1][HighIndex].Value = testString; });
            Assert.AreEqual(testString, message[1][HighIndex].Value);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Message_Timely_AddsLowIndexSegmentAndLowIndexField()
        {
            var testString = Randomized.String();
            var message = Message.Parse();
            var time = Measure.ExecutionTime(() => { message[100][1].Value = testString; });
            Assert.AreEqual(testString, message[100][1].Value);
            AssertTime.IsWithin(100, time);
        }

        [TestMethod]
        public void Message_Timely_AddsHighIndexSegmentAndField()
        {
            var testString = Randomized.String();
            var message = Message.Parse();
            var time = Measure.ExecutionTime(() => { message[HighIndex][HighIndex].Value = testString; });
            Assert.AreEqual(testString, message[HighIndex][HighIndex].Value);
            AssertTime.IsWithin(2000, time);
        }

        [TestMethod]
        public void Message_Timely_PopulatesSegments()
        {
            var testString = Randomized.String().Substring(0, 3).ToUpperInvariant() + "|";
            var message = Message.Parse();
            var time = Measure.ExecutionTime(() =>
            {
                for (var i = 1; i <= MediumIndex; i++)
                {
                    message[i].Value = testString;
                }
            });
            Assert.AreEqual(message[MediumIndex].Value, testString);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Message_Timely_SplitsSegmentsInLargeMessage()
        {
            IMessageParser message = null;
            var builder = new StringBuilder();
            builder.AppendLine(@"MSH|^~\&|");

            for (var i = 0; i < 100; i++)
            {
                builder.AppendLine("OBR|" + Randomized.String());
                for (var j = 0; j < 10; j++)
                {
                    builder.AppendLine("OBX|" + Randomized.String());
                    builder.AppendLine("OBX|" + Randomized.String());
                    builder.AppendLine("NTE|" + Randomized.String());
                }
            }
            Debug.WriteLine("Building...");
            Measure.ExecutionTime(() => { message = Message.Parse(builder.ToString()); });
            Debug.WriteLine("Splitting...");
            var time = Measure.ExecutionTime(() =>
            {
                var segments = message.SplitSegments("OBR");
                Assert.IsNotNull(segments);
            });
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Message_Timely_ProcessesManySmallMessages()
        {
            var time = Measure.ExecutionTime(() =>
            {
                var message = Message.Parse(ExampleMessages.A04);
                var dataField = message.Segments.OfType("IN1").First()[7][1][1];
                Assert.AreEqual("MUTUAL OF OMAHA", dataField.Value, @"Parsing IN1-7-1 failed.");
            }, 10000);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Message_Timely_ProcessesManyLargeMessages()
        {
            var time = Measure.ExecutionTime(() =>
            {
                var message = Message.Parse(ExampleMessages.MultipleObr);
                var dataField = message.Segments.OfType("OBR").First(s => s[1].Value == "4")[16][1][2];
                Assert.AreEqual("OLSTAD", dataField.Value, @"Parsing OBR4-16-2 failed.");
            }, 1000);
            AssertTime.IsWithin(1000, time);
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