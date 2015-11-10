using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class ParserBaseFunctionalTests : ParsingTestFixture
    {
        [Test]
        public void Parser_CanFormat()
        {
            var param = Any.String();
            const string message = "{0}|{1}";
            Assert.AreEqual(Message.ParseFormat(message, ExampleMessages.Minimum, param).Value,
                string.Format(message, ExampleMessages.Minimum, param));
        }

        [Test]
        public void Parser_DeleteThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.Delete(-1));
        }

        [Test]
        public void Parser_MoveThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.Move(2, -1));
            AssertAction.Throws<ElementException>(() => message.Move(-1, 2));
        }

        [Test]
        public void Parser_InsertThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.Insert(-1, Any.String())); 
        }

        [Test]
        public void Parser_InsertElementThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            var element = message[1];
            AssertAction.Throws<ElementException>(() => message.Insert(-1, element));
        }

        [Test]
        public void Parser_GetsNextIndex()
        {
            var message = Message.Parse(Any.Message());
            Assert.AreEqual(message.ValueCount + 1, message.NextIndex);
        }
    }
}
