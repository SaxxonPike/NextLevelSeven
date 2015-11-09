using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class MessageExtensionFunctionalTests : ParsingTestFixture
    {
        [Test]
        public void MessageExtensions_Builder_CanFilterPid_FromMessage()
        {
            var message = Message.Build(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptType("PID").Any(), "Only PIDs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptType("PID").All(s => s.Type != "PID"),
                "PIDs were not completely filtered.");
        }

        [Test]
        public void MessageExtensions_Parser_CanFilterPid_FromMessage()
        {
            var message = Message.Parse(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptType("PID").Any(), "Only PIDs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptType("PID").All(s => s.Type != "PID"),
                "PIDs were not completely filtered.");
        }

        [Test]
        public void MessageExtensions_Builder_CanFilterPidAndObx_FromMessage()
        {
            var message = Message.Build(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptTypes("PID", "OBX").Any(), "Only PIDs and OBXs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptTypes("PID", "OBX").All(s => s.Type != "PID" && s.Type != "OBX"),
                "PIDs and OBXs were not completely filtered.");
        }

        [Test]
        public void MessageExtensions_Parser_CanFilterPidAndObx_FromMessage()
        {
            var message = Message.Parse(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptTypes(new [] {"PID", "OBX"}.AsEnumerable()).Any(), "Only PIDs and OBXs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptTypes(new[] { "PID", "OBX" }.AsEnumerable()).All(s => s.Type != "PID" && s.Type != "OBX"),
                "PIDs and OBXs were not completely filtered.");
        }

        [Test]
        public void MessageExtensions_Builder_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Build(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            Assert.AreEqual(message.Segments.OfType("OBR").Count(), splits.Count(),
                "OBR split count doesn't match number of OBR segments.");
        }

        [Test]
        public void MessageExtensions_Parser_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            Assert.AreEqual(message.Segments.OfType("OBR").Count(), splits.Count(),
                "OBR split count doesn't match number of OBR segments.");
        }

        [Test]
        public void MessageExtensions_Builder_CanGetObrSplitsWithExtras()
        {
            var message = Message.Build(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message.Segments.OfType("OBR").Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }

        [Test]
        public void MessageExtensions_Parser_CanGetObrSplitsWithExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message.Segments.OfType("OBR").Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }
    }
}