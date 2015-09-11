using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeExampleMessageTests : NativeTestFixture
    {
        [TestMethod]
        public void ExampleMessage_HasProperSampleData()
        {
            var obrMessage = Message.Create(ExampleMessages.MultipleObr);
            Assert.IsTrue(obrMessage["OBR"].Count() > 1, "Sample multiple OBR data is bad: needs multiple OBR segments.");
        }
    }
}