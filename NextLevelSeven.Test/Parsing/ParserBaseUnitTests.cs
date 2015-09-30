using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class ParserBaseUnitTests : ParsingTestFixture
    {
        [TestMethod]
        public void Parsers_HashCodeComesFromString()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            Assert.AreEqual(message.GetHashCode(), ExampleMessages.Minimum.GetHashCode());
        }

        [TestMethod]
        public void Parsers_AreEquivalentWhenValuesMatch()
        {
            var segment0 = Message.Parse(ExampleMessages.Standard)[1];
            var segment1 = Message.Parse(ExampleMessages.Standard)[1];
            Assert.AreNotSame(segment0, segment1);
            Assert.IsTrue(segment0.Equals(segment1));
        }

        [TestMethod]
        public void Parsers_AreEquivalentWhenReferencesMatch()
        {
            var segment = Message.Parse(ExampleMessages.Standard)[1];
            var generic = (object) segment;
            Assert.IsTrue(segment.Equals(generic));
        }

        [TestMethod]
        public void Parsers_AreEquivalentToStrings()
        {
            var segment0 = Message.Parse(ExampleMessages.Standard)[1];
            Assert.IsTrue(segment0.Equals(segment0.Value));
        }

        [TestMethod]
        public void Parsers_AreNotEquivalentWhenNull()
        {
            var field = Message.Parse(ExampleMessages.Standard)[1][3];
            field.Value = null;
            Assert.IsFalse(field.Equals(null));
        }

    }
}
