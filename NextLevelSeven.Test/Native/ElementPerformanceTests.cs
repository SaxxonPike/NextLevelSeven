using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class ElementPerformanceTests
    {
        private const int MediumIndex = 1000;
        private const int HighIndex = 1000000;

        [TestMethod]
        public void Element_Timely_AddsHighIndexSegment()
        {
            var testString = Randomized.String();
            var message = Message.Create();
            var time = Measure.ExecutionTime(() => { message[HighIndex].Value = testString; });
            Assert.AreEqual(testString, message[HighIndex].Value);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Element_Timely_AddsHighIndexField()
        {
            var testString = Randomized.String();
            var message = Message.Create();
            var time = Measure.ExecutionTime(() => { message[1][HighIndex].Value = testString; });
            Assert.AreEqual(message[1][HighIndex], testString);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Element_Timely_AddsLowIndexSegmentAndHighIndexField()
        {
            var testString = Randomized.String();
            var message = Message.Create();
            var time = Measure.ExecutionTime(() => { message[2][HighIndex].Value = testString; });
            Assert.AreEqual(testString, message[2][HighIndex].Value);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Element_Timely_AddsLowIndexSegmentAndLowIndexField()
        {
            var testString = Randomized.String();
            var message = Message.Create();
            var time = Measure.ExecutionTime(() => { message[100][1].Value = testString; });
            Assert.AreEqual(testString, message[100][1].Value);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Element_Timely_AddsHighIndexSegmentAndField()
        {
            var testString = Randomized.String();
            var message = Message.Create();
            var time = Measure.ExecutionTime(() => { message[HighIndex][HighIndex].Value = testString; });
            Assert.AreEqual(testString, message[HighIndex][HighIndex].Value);
            AssertTime.IsWithin(2000, time);
        }

        [TestMethod]
        public void Element_Timely_PopulatesMessageSegments()
        {
            var testString = Randomized.String().Substring(0, 3).ToUpperInvariant() + "|";
            var message = Message.Create();
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
        public void Element_Timely_PopulatesMessageFields()
        {
            var message = Message.Create();
            var time = Measure.ExecutionTime(() =>
            {
                var msh = message[1];
                for (var i = 3; i <= 1000; i++)
                {
                    var id = Randomized.String();
                    msh[i].Value = id;
                    Assert.AreEqual(msh[i].Value, id);
                }
            });
            AssertTime.IsWithin(2000, time);
        }

        [TestMethod]
        public void Element_Timely_ReadsLastFieldInLargeMessage()
        {
            var message = Message.Create();
            string field = null;
            string value = Randomized.String();
            message[1][HighIndex].Value = value;
            var time = Measure.ExecutionTime(() => { field = message[1][HighIndex].Value; });
            Assert.AreEqual(value, field);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Element_Timely_ReadsFirstFieldInLargeMessage()
        {
            var message = Message.Create();
            string field = null;
            message[1][HighIndex].Value = Randomized.String();
            var time = Measure.ExecutionTime(() => { field = message[1][3].Value; });
            Assert.AreEqual(null, field);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Element_Timely_ReadsTwoFieldsInLargeMessage()
        {
            var message = Message.Create();
            string field = null;
            message[1][HighIndex].Value = Randomized.String();
            var time = Measure.ExecutionTime(() =>
            {
                field = message[1][HighIndex - 1].Value;
                field = message[1][3].Value;
            });
            Assert.AreEqual(null, field);
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Element_Timely_ModifiesFirstFieldInLargeMessage()
        {
            var message = Message.Create();
            message[1][HighIndex].Value = "test";
            var time = Measure.ExecutionTime(() => { message[1][3].Value = "test2"; });
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Element_Timely_SplitsSegmentsInLargeMessage()
        {
            var message = Message.Create();
            for (var i = 0; i < 100; i++)
            {
                message.Add("OBR|" + Randomized.String());
                for (var j = 0; j < 10; j++)
                {
                    message.Add("OBX|" + Randomized.String());
                    message.Add("OBX|" + Randomized.String());
                    message.Add("NTE|" + Randomized.String());
                }
            }
            var time = Measure.ExecutionTime(() =>
            {
                var segments = message.SplitSegments("OBR");
                Assert.IsNotNull(segments);
            });
            AssertTime.IsWithin(500, time);
        }

        [TestMethod]
        public void Element_Timely_ProcessesManySmallMessages()
        {
            var time = Measure.ExecutionTime(() =>
            {
                var message = Message.Create(ExampleMessages.A04);
                var dataField = message["IN1"].First()[7][0][1];
                Assert.AreEqual("MUTUAL OF OMAHA", dataField.Value, @"Parsing IN1-7-1 failed.");
            }, 10000);
            AssertTime.IsWithin(1000, time);
        }

        [TestMethod]
        public void Element_Timely_ProcessesManyLargeMessages()
        {
            var time = Measure.ExecutionTime(() =>
            {
                var message = Message.Create(ExampleMessages.MultipleObr);
                var dataField = message["OBR"].First(s => s[1].Value == "4")[16][0][2];
                Assert.AreEqual("OLSTAD", dataField.Value, @"Parsing OBR4-16-2 failed.");
            }, 1000);
            AssertTime.IsWithin(1000, time);
        }
    }
}