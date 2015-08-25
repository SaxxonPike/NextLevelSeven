using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ExampleMessageTests
    {
        [TestInitialize]
        public void MessageExtensions_HasProperSampleData()
        {
            var obrMessage = new Message(ExampleMessages.MultipleObr);
            Assert.IsTrue(obrMessage["OBR"].Count() > 1, "Sample multiple OBR data is bad: needs multiple OBR segments.");
        }
    }
}
