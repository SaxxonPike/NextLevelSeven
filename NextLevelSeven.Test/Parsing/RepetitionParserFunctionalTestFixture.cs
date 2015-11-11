using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class RepetitionParserFunctionalTestFixture : DescendantElementParserBaseTestFixture<IRepetitionParser, IRepetition>
    {
        protected override IRepetitionParser BuildParser()
        {
            return Message.Parse(ExampleMessageRepository.Minimum)[1][3][1];
        }

        [Test]
        public void Repetition_CanMoveComponents()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1];
            element.Values = new[] {Any.String(), Any.String(), Any.String(), Any.String()};
            var newMessage = element.Clone();
            newMessage[2].Move(3);
            newMessage[3].Value.Should().Be(element[2].Value);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Repetition_Throws_WhenIndexedBelowOne()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[1][3][1];
            element[0].Value.Should().BeNull();
        }

        [Test]
        public void Repetition_CanAddDescendantsAtEnd()
        {
            var repetition = Message.Parse(ExampleMessageRepository.Standard)[2][3][4];
            var count = repetition.ValueCount;
            var id = Any.String();
            repetition[count + 1].Value = id;
            repetition.ValueCount.Should().Be(count + 1);
        }

        [Test]
        public void Repetition_CanGetComponents()
        {
            var repetition = Message.Parse(ExampleMessageRepository.Standard)[8][13][2];
            repetition.Components.Count().Should().Be(3);
        }

        [Test]
        public void Repetition_CanGetComponentsGenerically()
        {
            IRepetition repetition = Message.Parse(ExampleMessageRepository.Standard)[8][13][2];
            repetition.Components.Count().Should().Be(3);
        }

        [Test]
        public void Repetition_CanGetComponentsByIndexer()
        {
            var component = Message.Parse(ExampleMessageRepository.Standard)[8][13][2][2];
            component.Value.Should().Be("ORN");
        }

        [Test]
        public void Repetition_CanDeleteComponent()
        {
            var message = Message.Parse("MSH|^~\\&|\rTST|123^456~789^012");
            var component = message[2][1][2];
            ElementExtensions.Delete(component, 1);
            message.Value.Should().Be("MSH|^~\\&|\rTST|123^456~012");
        }

        [Test]
        public void Repetition_CanWriteStringValue()
        {
            var repetition = Message.Parse(ExampleMessageRepository.Standard)[1][3][1];
            var value = Any.String();
            repetition.Value = value;
            repetition.Value.Should().Be(value);
        }

        [Test]
        public void Repetition_CanWriteNullValue()
        {
            var repetition = Message.Parse(ExampleMessageRepository.Standard)[1][3][1];
            var value = Any.String();
            repetition.Value = value;
            repetition.Value = null;
            repetition.Value.Should().BeNull();
        }
    }
}