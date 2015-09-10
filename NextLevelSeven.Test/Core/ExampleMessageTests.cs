using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ExampleMessageTests
    {
        [TestMethod]
        public void ExampleMessage_HasProperSampleData()
        {
            var obrMessage = Message.Create(ExampleMessages.MultipleObr);
            Assert.IsTrue(obrMessage["OBR"].Count() > 1, "Sample multiple OBR data is bad: needs multiple OBR segments.");
        }
    }
}
