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
            Assert.AreEqual(field.As.DateTime, new DateTime(2013, 05, 28, 07, 38, 29));
        }

        [TestMethod]
        public void Element_CanGetDescendants()
        {
            var segment = new Message(ExampleMessages.Standard)[2];
            var test = segment[2];
            Assert.AreEqual(test.Value, @"20130528073829");
        }

    }
}
