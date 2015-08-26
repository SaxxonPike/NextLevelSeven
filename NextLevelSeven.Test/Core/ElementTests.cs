using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ElementTests
    {
        [TestMethod]
        public void Element_CanAddElementsAtEnd()
        {
            var segment = new Message(ExampleMessages.Standard)[2];
            var fieldCount = segment.DescendantCount;
            segment[fieldCount + 1].Value = "test";
            Assert.AreEqual(fieldCount + 1, segment.DescendantCount,
                @"Number of elements after appending at the end is incorrect.");
        }

        [TestMethod]
        public void Element_CanBeCloned()
        {
            var segment = new Message(ExampleMessages.Standard)[2];
            var clone = segment.CloneDetached();
            Assert.AreNotSame(segment, clone, @"Segment and clone are referencing the same object.");
            Assert.AreEqual(segment.Value, clone.Value);
        }

        [TestMethod]
        public void Element_WithIdenticalValueToAnotherElement_IsEquivalent()
        {
            var segment1 = new Message(ExampleMessages.Standard)[1];
            var segment2 = new Message(ExampleMessages.Standard)[1];
            Assert.AreEqual(segment1.Value, segment2.Value);
        }

        [TestMethod]
        public void Element_CanConvertDateTimes()
        {
            var field = new Message(ExampleMessages.Standard)[2][2];
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), field.As.DateTime);
        }

        [TestMethod]
        public void Element_CanConvertPartialDateTimes()
        {
            var field = new Message(ExampleMessages.VersionlessMessage)[1][7];
            field.Value = "20130528073829";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), field.As.DateTime, "DateTime was not converted correctly.");
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
            field.Value = "";
            It.Throws<ArgumentException>(() => { var dt = field.As.DateTime; });
        }

        [TestMethod]
        public void Element_CanGetDescendants()
        {
            var message = new Message(ExampleMessages.Standard);
            var segment = message[2];
            var field = segment[2];
            Assert.AreEqual(@"20130528073829", field.Value);
        }

    }
}
