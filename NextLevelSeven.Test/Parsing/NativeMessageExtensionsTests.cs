using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeMessageExtensionsTests : NativeTestFixture
    {
        [TestMethod]
        public void MessageExtensions_CanFilterPid_FromMessage()
        {
            var message = Message.Parse(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.Segments.ExceptType("PID").Any(), "Only PIDs are to be filtered.");
            Assert.IsTrue(message.Segments.ExceptType("PID").All(s => s.Type != "PID"),
                "PIDs were not completely filtered.");
        }

        [TestMethod]
        public void MessageExtensions_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            Assert.AreEqual(message["OBR"].Count(), splits.Count(),
                "OBR split count doesn't match number of OBR segments.");
        }

        [TestMethod]
        public void MessageExtensions_CanGetObrSplitsWithExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message["OBR"].Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }
    }
}