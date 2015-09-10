using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class ElementTests
    {
        [TestMethod]
        public void Element_CanAddDescendantsAtEnd()
        {
            var segment = Message.Create(ExampleMessages.Standard)[2];
            var fieldCount = segment.DescendantCount;
            segment[fieldCount + 1].Value = "test";
            Assert.AreEqual(fieldCount + 1, segment.DescendantCount,
                @"Number of elements after appending at the end is incorrect.");
        }

        [TestMethod]
        public void Element_CanAddDescendantsBeyondEnd()
        {
            var segment = Message.Create(ExampleMessages.Standard)[2];
            var fieldCount = segment.DescendantCount;
            segment[fieldCount + 2].Value = "test";
            Assert.AreEqual(fieldCount + 2, segment.DescendantCount,
                @"Number of elements after appending at the end is incorrect.");
        }

        [TestMethod]
        public void Element_CanBeCloned()
        {
            var segment = Message.Create(ExampleMessages.Standard)[2];
            var clone = segment.CloneDetached();
            Assert.AreNotSame(segment, clone, @"Segment and clone are referencing the same object.");
            Assert.AreEqual(segment.Value, clone.Value);
        }

        [TestMethod]
        public void Element_WithIdenticalValueToAnotherElement_IsEquivalent()
        {
            var segment1 = Message.Create(ExampleMessages.Standard)[1];
            var segment2 = Message.Create(ExampleMessages.Standard)[1];
            Assert.AreEqual(segment1.Value, segment2.Value);
        }

        [TestMethod]
        public void Element_CanConvertDateTimes()
        {
            var field = Message.Create(ExampleMessages.Standard)[2][2];
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), field.As.DateTime);
        }

        [TestMethod]
        public void Element_CanConvertPartialDateTimes()
        {
            var field = Message.Create(ExampleMessages.VersionlessMessage)[1][7];
            field.Value = "20130528073829";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), field.As.DateTime,
                "DateTime was not converted correctly.");
            field.Value = "201305280738";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 00), field.As.DateTime, "Second didn't default to zero.");
            field.Value = "2013052807";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 00, 00), field.As.DateTime, "Minute didn't default to zero.");
            field.Value = "20130528";
            Assert.AreEqual(new DateTime(2013, 05, 28, 00, 00, 00), field.As.DateTime, "Hour didn't default to zero.");
            field.Value = "201305";
            Assert.AreEqual(new DateTime(2013, 05, 01, 00, 00, 00), field.As.DateTime, "Day didn't default to one.");
            field.Value = "2013";
            Assert.AreEqual(new DateTime(2013, 01, 01, 00, 00, 00), field.As.DateTime, "Month didn't default to one.");
            field.Value = "201";
            It.Throws<ArgumentException>(() => Assert.IsNotNull(field.As.DateTime),
                "Conversion must fail with too short of a year.");
            field.Value = "";
            Assert.IsNull(field.As.DateTime, "Empty or null input values must return null.");
        }

        [TestMethod]
        public void Element_CanGetDescendants()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var segment = message[2];
            var field = segment[2];
            Assert.AreEqual(@"20130528073829", field.Value);
        }

        [TestMethod]
        public void Element_WillHaveValuesInterpretedAsNull()
        {
            var message = Message.Create();
            message[1][3].Value = "\"\"";
            Assert.AreEqual(message[1][3].Value, null, @"Value of two double quotes was not interpreted as null.");
            Assert.IsTrue(message[1][3].Exists, @"Explicitly set null value must appear to exist.");
        }

        [TestMethod]
        public void Element_WillConsiderNonPresentValuesToNotExist()
        {
            var message = Message.Create();
            Assert.IsFalse(message[2].Exists, @"Nonexistant segment is marked as existing.");
        }

        [TestMethod]
        public void Element_WithNoSignificantDescendants_ShouldNotClaimToHaveSignificantDescendants()
        {
            var message = Message.Create();
            Assert.IsFalse(message[1][3].HasSignificantDescendants,
                @"Element claims to have descendants when it should not.");
        }

        [TestMethod]
        public void Element_WithSignificantDescendants_ShouldClaimToHaveSignificantDescendants()
        {
            var message = Message.Create();
            Assert.IsTrue(message[1].HasSignificantDescendants,
                @"Segment claims to not have descendants when it should.");
        }

        [TestMethod]
        public void Element_WillPointToCorrectValue_WhenOtherValuesChange()
        {
            var message = Message.Create();
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var expected = Randomized.String().Substring(0, 5);

            msh4.Value = expected;
            msh3.Value = Randomized.String();
            Assert.AreEqual(msh4.Value, expected);
        }

        [TestMethod]
        public void Element_WillPointToCorrectValue_WhenAncestorChanges()
        {
            var message = Message.Create(String.Format(@"MSH|^~\&|{0}|{1}", Randomized.String(), Randomized.String()));
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var newMsh3Value = Randomized.String();
            var newMsh4Value = Randomized.String();

            message.Value = String.Format(@"MSH|^~\&|{0}|{1}", newMsh3Value, newMsh4Value);
            Assert.AreEqual(newMsh3Value, msh3.Value, @"MSH-3 was not the expected value after changing MSH.");
            Assert.AreEqual(newMsh4Value, msh4.Value, @"MSH-4 was not the expected value after changing MSH.");
        }

        [TestMethod]
        public void Element_ValuesReturnsProperlySplitData()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var segmentStrings = message.Value.Split('\xD');
            var segments = message.Values.ToList();

            Assert.AreEqual(segmentStrings.Length, segments.Count,
                @"Splitting main value and calling Values returns different element counts.");

            for (var i = 0; i < segments.Count; i++)
            {
                Assert.AreEqual(segments[i], segmentStrings[i], @"Values are not equal.");
            }
        }

        [TestMethod]
        public void Element_CanDeleteComponent()
        {
            var message = Message.Create("MSH|^~\\&|\rTST|123^456~789^012");
            var component = message[2][1][2];
            component.Delete(1);
            Assert.AreEqual("MSH|^~\\&|\rTST|123^456~012", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Element_CanDeleteDescendants()
        {
            var message = Message.Create("MSH|^~\\&|1|2|3|4|5");
            var segment = message[1];
            segment.DescendantElements.Skip(2).Where(i => i.As.Int%2 == 0).Delete();
            Assert.AreEqual("MSH|^~\\&|1|3|5", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Element_CanDeleteField()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var segment = message[1];
            var field3 = segment[3].Value;
            var field5 = segment[5].Value;
            var field6 = segment[6].Value;
            segment.Delete(4);
            Assert.AreEqual(field3, segment[3].Value, @"Expected segment[3] to remain the same after delete.");
            Assert.AreEqual(field5, segment[4].Value, @"Expected segment[5] to become segment[4].");
            Assert.AreEqual(field6, segment[5].Value, @"Expected segment[6] to become segment[5].");
        }

        [TestMethod]
        public void Element_CanDeleteRepetition()
        {
            var message = Message.Create("MSH|^~\\&|\rTST|123~456|789~012");
            var field = message[2][1];
            field.Delete(1);
            Assert.AreEqual("MSH|^~\\&|\rTST|456|789~012", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Element_CanDeleteSegment()
        {
            var message = Message.Create(ExampleMessages.Standard);
            var segment1 = message[1].Value;
            var segment3 = message[3].Value;
            var segment4 = message[4].Value;
            message.Delete(2);
            Assert.AreEqual(segment1, message[1].Value, @"Expected message[1] to remain the same after delete.");
            Assert.AreEqual(segment3, message[2].Value, @"Expected message[3] to become message[2].");
            Assert.AreEqual(segment4, message[3].Value, @"Expected message[4] to become message[3].");
        }

        [TestMethod]
        public void Element_CanDeleteSubcomponent()
        {
            var message = Message.Create("MSH|^~\\&|\rTST|123^456&ABC~789^012");
            var component = message[2][1][1][2];
            component.Delete(1);
            Assert.AreEqual("MSH|^~\\&|\rTST|123^ABC~789^012", message.Value, @"Message was modified unexpectedly.");
        }
    }
}