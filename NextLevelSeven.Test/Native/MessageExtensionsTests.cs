using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class MessageExtensionsTests
    {
        [TestMethod]
        public void MessageExtensions_CanFilterPid_FromMessage()
        {
            var message = Message.Create(ExampleMessages.MultiplePid);
            Assert.IsTrue(message.ExcludeSegments("PID").Any(), "Only PIDs are to be filtered.");
            Assert.IsTrue(message.ExcludeSegments("PID").All(s => s.Type != "PID"), "PIDs were not completely filtered.");
        }

        [TestMethod]
        public void MessageExtensions_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Create(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            Assert.AreEqual(message["OBR"].Count(), splits.Count(),
                "OBR split count doesn't match number of OBR segments.");
        }

        [TestMethod]
        public void MessageExtensions_CanGetObrSplitsWithExtras()
        {
            var message = Message.Create(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message["OBR"].Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }
    }
}