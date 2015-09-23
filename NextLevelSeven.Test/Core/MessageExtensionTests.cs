using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Parsing;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class MessageExtensionTests : ParsingTestFixture
    {
        [TestMethod]
        public void MessageExtensions_Builder_CanFilterPid_FromMessage()
        {
            var message = Message.Build(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptType("PID").Any(), "Only PIDs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptType("PID").All(s => s.Type != "PID"),
                "PIDs were not completely filtered.");
        }

        [TestMethod]
        public void MessageExtensions_Parser_CanFilterPid_FromMessage()
        {
            var message = Message.Parse(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptType("PID").Any(), "Only PIDs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptType("PID").All(s => s.Type != "PID"),
                "PIDs were not completely filtered.");
        }

        [TestMethod]
        public void MessageExtensions_Builder_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Build(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            Assert.AreEqual(message.Segments.OfType("OBR").Count(), splits.Count(),
                "OBR split count doesn't match number of OBR segments.");
        }

        [TestMethod]
        public void MessageExtensions_Parser_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            Assert.AreEqual(message.Segments.OfType("OBR").Count(), splits.Count(),
                "OBR split count doesn't match number of OBR segments.");
        }

        [TestMethod]
        public void MessageExtensions_Builder_CanGetObrSplitsWithExtras()
        {
            var message = Message.Build(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message.Segments.OfType("OBR").Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }

        [TestMethod]
        public void MessageExtensions_Parser_CanGetObrSplitsWithExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message.Segments.OfType("OBR").Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }
    }
}