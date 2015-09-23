using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class SubcomponentParserTests : ParsingTestFixture
    {
        [TestMethod]
        public void Subcomponent_CanBeCloned()
        {
            var subcomponent = Message.Parse(ExampleMessages.Standard)[1][3][1][1][1];
            var clone = subcomponent.Clone();
            Assert.AreNotSame(subcomponent, clone, "Cloned subcomponent is the same referenced object.");
            Assert.AreEqual(subcomponent.Value, clone.Value, "Cloned subcomponent has different contents.");
        }

        [TestMethod]
        public void Subcomponent_CanAddDescendantsAtEnd()
        {
            var subcomponent = Message.Parse(ExampleMessages.Standard)[2][3][4][1];
            var count = subcomponent.ValueCount;
            var id = Randomized.String();
            subcomponent[count + 1].Value = id;
            Assert.AreEqual(count + 1, subcomponent.ValueCount,
                @"Number of elements after appending at the end of a subcomponent is incorrect.");
        }

        [TestMethod]
        public void Subcomponent_CanWriteStringValue()
        {
            var subcomponent = Message.Parse(ExampleMessages.Standard)[1][3][1][1][1];
            var value = Randomized.String();
            subcomponent.Value = value;
            Assert.AreEqual(value, subcomponent.Value, "Value mismatch after write.");
        }

        [TestMethod]
        public void Subcomponent_CanWriteNullValue()
        {
            var subcomponent = Message.Parse(ExampleMessages.Standard)[1][3][1][1][1];
            var value = Randomized.String();
            subcomponent.Value = value;
            subcomponent.Value = null;
            Assert.IsNull(subcomponent.Value, "Value mismatch after write.");
        }
    }
}