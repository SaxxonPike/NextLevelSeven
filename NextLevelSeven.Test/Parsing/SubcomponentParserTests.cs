using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class SubcomponentParserTests : ParsingTestFixture
    {
        [TestMethod]
        public void Subcomponent_HasComponentAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1][1];
            Assert.IsNotNull(element.Ancestor);
        }

        [TestMethod]
        public void Subcomponent_HasGenericComponentAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1][1] as ISubcomponent;
            Assert.IsNotNull(element.Ancestor);
        }

        [TestMethod]
        public void Subcomponent_HasGenericAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1][1] as IElementParser;
            Assert.IsNotNull(element.Ancestor as IComponent);
        }

        [TestMethod]
        public void Subcomponent_HasOneValue()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1][1];
            var val0 = Randomized.String();
            element.Value = val0;
            Assert.AreEqual(1, element.ValueCount);
            Assert.AreEqual(element.Value, val0);
            Assert.AreEqual(1, element.Values.Count());
        }

        [TestMethod]
        public void Subcomponent_ThrowsWhenMovingElements()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1][1];
            element.Value = Randomized.String();
            var newMessage = element.Clone();
            It.Throws<ParserException>(() => newMessage[2].Move(3));
            Assert.AreEqual(element.Value, newMessage.Value);
        }

        [TestMethod]
        public void Subcomponent_Throws_WhenIndexed()
        {
            var element = Message.Parse(ExampleMessages.Standard)[1][3][1][1][1];
            string value = null;
            It.Throws<ParserException>(() => { value = element[1].Value; });
            Assert.IsNull(value);
        }

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