using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Test.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class ParserBaseFunctionalTestFixture : ParsingBaseTestFixture
    {
        [Test]
        public void Parser_CanFormat()
        {
            var param = Any.String();
            const string message = "{0}|{1}";
            Message.ParseFormat(message, ExampleMessageRepository.Minimum, param).RawValue
                .Should().Be(string.Format(message, ExampleMessageRepository.Minimum, param));
        }

        [Test]
        public void Parser_DeleteThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            message.Invoking(m => m.Delete(-1)).Should().Throw<ElementException>();
        }

        [Test]
        [TestCase(2, -1)]
        [TestCase(-1, 2)]
        public void Parser_MoveThrowsIfInvalidIndex(int from, int to)
        {
            var message = Message.Parse(Any.Message())[2][1];
            message.Invoking(m => m.Move(from, to)).Should().Throw<ElementException>();
        }

        [Test]
        public void Parser_InsertThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            message.Invoking(m => m.Insert(-1, Any.String())).Should().Throw<ElementException>(); 
        }

        [Test]
        public void Parser_InsertElementThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Any.Message())[2][1];
            var element = message[1];
            message.Invoking(m => m.Insert(-1, element)).Should().Throw<ElementException>();
        }

        [Test]
        public void Parser_GetsNextIndex()
        {
            var message = Message.Parse(Any.Message());
            message.NextIndex.Should().Be(message.ValueCount + 1);
        }
    }
}
