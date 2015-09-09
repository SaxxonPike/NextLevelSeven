using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ExampleMessageTests
    {
        [TestMethod]
        public void ExampleMessage_HasProperSampleData()
        {
            var obrMessage = new NativeMessage(ExampleMessages.MultipleObr);
            Assert.IsTrue(obrMessage["OBR"].Count() > 1, "Sample multiple OBR data is bad: needs multiple OBR segments.");
        }
    }
}
