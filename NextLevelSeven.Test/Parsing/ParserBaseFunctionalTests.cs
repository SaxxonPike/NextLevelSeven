using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class ParserBaseFunctionalTests : ParsingTestFixture
    {
        [TestMethod]
        public void Parser_CanFormat()
        {
            var param = Mock.String();
            const string message = "{0}|{1}";
            Assert.AreEqual(Message.ParseFormat(message, ExampleMessages.Minimum, param).Value,
                string.Format(message, ExampleMessages.Minimum, param));
        }

        [TestMethod]
        public void Parser_DeleteThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.Delete(-1));
        }

        [TestMethod]
        public void Parser_MoveThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.Move(2, -1));
            AssertAction.Throws<ElementException>(() => message.Move(-1, 2));
        }

        [TestMethod]
        public void Parser_InsertThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.Insert(-1, Mock.String())); 
        }

        [TestMethod]
        public void Parser_InsertElementThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            var element = message[1];
            AssertAction.Throws<ElementException>(() => message.Insert(-1, element));
        }

        [TestMethod]
        public void Parser_GetsNextIndex()
        {
            var message = Message.Parse(Mock.Message());
            Assert.AreEqual(message.ValueCount + 1, message.NextIndex);
        }
    }
}
