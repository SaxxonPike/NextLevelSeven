using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeElementTests : NativeTestFixture
    {
        [TestMethod]
        public void Element_CanConvertDateTimes()
        {
            var field = Message.Create(ExampleMessages.Standard)[2][2];
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), field.As.DateTime);
        }

        [TestMethod]
        public void Element_CanConvertPartialDateTimes()
        {
            var field = Message.Create(ExampleMessages.VersionlessMessage)[1][7];
            field.Value = "20130528073829";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29), field.As.DateTime,
                "DateTime was not converted correctly.");
            field.Value = "201305280738";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 00), field.As.DateTime, "Second didn't default to zero.");
            field.Value = "2013052807";
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 00, 00), field.As.DateTime, "Minute didn't default to zero.");
            field.Value = "20130528";
            Assert.AreEqual(new DateTime(2013, 05, 28, 00, 00, 00), field.As.DateTime, "Hour didn't default to zero.");
            field.Value = "201305";
            Assert.AreEqual(new DateTime(2013, 05, 01, 00, 00, 00), field.As.DateTime, "Day didn't default to one.");
            field.Value = "2013";
            Assert.AreEqual(new DateTime(2013, 01, 01, 00, 00, 00), field.As.DateTime, "Month didn't default to one.");
            field.Value = "201";
            It.Throws<ArgumentException>(() => Assert.IsNotNull(field.As.DateTime),
                "Conversion must fail with too short of a year.");
            field.Value = "";
            Assert.IsNull(field.As.DateTime, "Empty or null input values must return null.");
        }
    }
}