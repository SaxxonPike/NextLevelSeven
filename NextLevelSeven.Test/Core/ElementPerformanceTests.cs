using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ElementPerformanceTests
    {
        private const int MediumIndex = 1000;
        private const int HighIndex = 1000000;

        void AssertInconclusiveIfSlow(long tolerance, long measured)
        {
            if (measured > tolerance)
            {
                Assert.Inconclusive("Test was slow. Measured: {0}ms. Tolerance: {1}ms.", measured, tolerance);
            }
        }

        [TestMethod]
        public void Element_Timely_AddsHighIndexSegment()
        {
            var testString = Randomized.String();
            var message = new Message();
            var time = Measure.ExecutionTime(() =>
            {
                message[HighIndex].Value = testString;
            });
            Assert.AreEqual(message[HighIndex], testString);
        }

        [TestMethod]
        public void Element_Timely_AddsHighIndexField()
        {
            var testString = Randomized.String();
            var message = new Message();
            var time = Measure.ExecutionTime(() =>
            {
                message[1][HighIndex].Value = testString;
            });
            Assert.AreEqual(message[1][HighIndex], testString);
            AssertInconclusiveIfSlow(1000, time);
        }

        [TestMethod]
        public void Element_Timely_AddsLowIndexSegmentAndHighIndexField()
        {
            var testString = Randomized.String();
            var message = new Message();
            var time = Measure.ExecutionTime(() =>
            {
                message[2][HighIndex].Value = testString;
            });
            Assert.AreEqual(testString, message[2][HighIndex].Value);
            AssertInconclusiveIfSlow(1000, time);
        }

        [TestMethod]
        public void Element_Timely_AddsHighIndexSegmentAndField()
        {
            var testString = Randomized.String();
            var message = new Message();
            var time = Measure.ExecutionTime(() =>
            {
                message[HighIndex][HighIndex].Value = testString;
            });
            Assert.AreEqual(testString, message[HighIndex][HighIndex].Value);
            AssertInconclusiveIfSlow(2000, time);
        }

        [TestMethod]
        public void Element_Timely_PopulatesMessageSegments()
        {
            var testString = Randomized.String().Substring(0, 3).ToUpperInvariant() + "|";
            var message = new Message();
            var time = Measure.ExecutionTime(() =>
            {
                for (var i = 1; i <= MediumIndex; i++)
                {
                    message[i].Value = testString;
                }
            });
            Assert.AreEqual(message[MediumIndex].Value, testString);
            AssertInconclusiveIfSlow(1000, time);
        }

        [TestMethod]
        public void Element_Timely_PopulatesMessageFields()
        {
            var message = new Message();
            var time = Measure.ExecutionTime(() =>
            {
                var msh = message[1];
                for (var i = 1; i <= 1000; i++)
                {
                    msh[i].Value = "test";                    
                }
            });
            AssertInconclusiveIfSlow(1000, time);
        }

        [TestMethod]
        public void Element_Timely_ReadsLastFieldInLargeMessage()
        {
            var message = new Message();
            string field = null;
            string value = Randomized.String();
            message[1][HighIndex].Value = value;
            var time = Measure.ExecutionTime(() =>
            {
                field = message[1][HighIndex].Value;
            });
            Assert.AreEqual(value, field);
            AssertInconclusiveIfSlow(1000, time);
        }

        [TestMethod]
        public void Element_Timely_ReadsFirstFieldInLargeMessage()
        {
            var message = new Message();
            string field = null;
            message[1][HighIndex].Value = Randomized.String();
            var time = Measure.ExecutionTime(() =>
            {
                field = message[1][3].Value;
            });
            Assert.AreEqual(null, field);
            AssertInconclusiveIfSlow(500, time);
        }

        [TestMethod]
        public void Element_Timely_ModifiesFirstFieldInLargeMessage()
        {
            var message = new Message();
            message[1][HighIndex].Value = "test";
            var time = Measure.ExecutionTime(() =>
            {
                message[1][3].Value = "test2";
            });
            AssertInconclusiveIfSlow(500, time);
        }

    }
}
