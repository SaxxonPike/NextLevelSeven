using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class ElementExtensionFunctionalTestFixture : CoreBaseTestFixture
    {
        [Test]
        public void Simplify_HasCorrectNumberOfFields_AfterNullifyingField()
        {
            var message = Message.Parse(Any.Message());
            var segment = message.Segment(1);
            var fieldCount = segment.ValueCount;
            segment.Field(3).Nullify();
            segment.Simplified().Count().Should().Be(fieldCount - 1);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Insert_Throws_WhenAncestorIsNotPresentInsertingString()
        {
            var message = Message.Parse();
            var segment = message[1].Clone();
            segment.Insert(Any.String());
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Insert_Throws_WhenAncestorIsNotPresentInsertingElement()
        {
            var message = Message.Parse(Any.Message());
            var segment = message[1].Clone();
            segment.Insert(message[2]);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Nullify_Throws_WhenNullifyingMessage()
        {
            var message = Message.Parse(Any.Message());
            message.Nullify();
        }

        [Test]
        public void Nullify_CanNullifyNonEncodingValuesInMshSegment()
        {
            var message = Message.Parse(Any.Message());
            message[1].Nullify();
            message[1].ValueCount.Should().Be(3);
        }

        [Test]
        public void Nullify_CanNullifyNonMshSegments()
        {
            var message = Message.Parse(Any.Message());
            message[2].Nullify();
            message[2].ValueCount.Should().Be(1);
        }

        [Test]
        public void HasSignificantDescendants_ReturnsFalse_OnFieldDelimiter()
        {
            var message = Message.Parse(Any.Message());
            message[1][1].HasSignificantDescendants().Should().BeFalse();
        }

        [Test]
        public void HasSignificantDescendants_ReturnsFalse_OnEncodingField()
        {
            var message = Message.Parse(Any.Message());
            message[1][2].HasSignificantDescendants().Should().BeFalse();
        }

        [Test]
        public void AddRange_CanAddMultipleElements()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var message0 = Message.Build(ExampleMessageRepository.Standard);
            var message1 = message0.Clone();
            message0[1][3].Value = val0;
            message1[1][3].Value = val1;
            message0.AddRange(message1.Segments.Skip(2));
            message0.ValueCount.Should().Be(message1.ValueCount * 2 - 2);
            message0[message1.ValueCount + 1].Value.Should().Be(message1[3].Value);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(0, 3)]
        [TestCase(1, 0)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 3)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [ExpectedException(typeof(ElementException))]
        public void Move_Throws_WhenMovingEncodingFields(int from, int to)
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            message[1][from].Move(to);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Move_DoesNothing_WhenIndicesMatch(int index)
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            message[index].Move(index);
        }

        [Test]
        public void Delete_CanDeleteElementFromAncestor()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var newMessage = message.Clone();
            newMessage[2].Delete();
            newMessage[2].Value.Should().Be(message[3].Value);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Delete_Throws_WhenDeletingMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Delete();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Move_Throws_WhenMovingMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Move(2);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [ExpectedException(typeof(ElementException))]
        public void Move_Throws_WhenMovingToInvalidIndices(int index)
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message[2].Move(index);
        }

        [Test]
        public void Delete_DoesNothingWhenNoElementsArePresent()
        {
            Enumerable.Empty<IElement>().Delete();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Delete_Throws_WhenDeletingElementsFromDifferentAncestors()
        {
            var message0 = Message.Parse(ExampleMessageRepository.Standard);
            var message1 = Message.Parse(ExampleMessageRepository.Standard);
            var messageGroup = new[] {message0[2], message1[2]};
            messageGroup.Delete();
        }

        [Test]
        public void ToNewParser_CanCreateNewEmptyMessageFromZeroSegments()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var parser = message.Segments.OfType("|||").ToNewParser();
            parser.ValueCount.Should().Be(1);
            message[1].Type.Should().Be("MSH");
        }

        [Test]
        public void ToNewParser_Parser_CanCreateNewMessageFromSegments()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var parser = message.Segments.OfType("PID").ToNewParser();
            parser.ValueCount.Should().Be(3);
            parser[1].Value.Should().Be(message[1].Value);
            parser[2].Value.Should().Be(message.Segments.OfType("PID").First().Value);
        }

        [Test]
        public void OfTypes_CanGetMultipleSegmentTypes()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var segments = message.Segments.OfTypes("MSH", "PID");
            segments.Count().Should().Be(3);
        }

        [Test]
        public void OfTypes_CanGetMultipleSegmentTypesAsEnumerable()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var segments = message.Segments.OfTypes(new [] {"MSH", "PID"}.AsEnumerable());
            segments.Count().Should().Be(3);
        }

        [Test]
        public void Segment_CanGetSegment()
        {
            var parser = Message.Parse(ExampleMessageRepository.Variety);
            parser.Segment(1).Value.Should().Be(parser[1].Value)
                .And.Should().NotBeNull();
        }

        [Test]
        public void Field_CanGetField()
        {
            var parser = Message.Parse(ExampleMessageRepository.Variety);
            parser.Segment(1).Field(3).Value.Should().Be(parser[1][3].Value)
                .And.Should().NotBeNull();
        }

        [Test]
        public void Repetition_CanGetRepetition()
        {
            var parser = Message.Parse(ExampleMessageRepository.Variety);
            parser.Segment(1).Field(3).Repetition(2).Value.Should().Be(parser[1][3][2].Value)
                .And.Should().NotBeNull();
        }

        [Test]
        public void Component_CanGetComponent()
        {
            var parser = Message.Parse(ExampleMessageRepository.Variety);
            parser.Segment(1).Field(3).Repetition(2).Component(2).Value.Should().Be(parser[1][3][2][2].Value)
                .And.Should().NotBeNull();
        }

        [Test]
        public void Component_CanGetComponentThroughField()
        {
            var parser = Message.Parse(ExampleMessageRepository.Variety);
            parser.Segment(1).Field(3).Component(2).Value.Should().Be(parser[1][3][1][2].Value)
                .And.Should().NotBeNull();
        }

        [Test]
        public void Subcomponent_CanGetSubcomponent()
        {
            var parser = Message.Parse(ExampleMessageRepository.Variety);
            parser.Segment(1).Field(3).Repetition(2).Component(2).Subcomponent(2).Value
                .Should().Be(parser[1][3][2][2][2].Value)
                .And.Should().NotBeNull();
        }

        [Test]
        public void Delete_ShiftsContents()
        {
            var element = Message.Build(ExampleMessageRepository.Standard);
            var modifiedElement = element.Clone() as IElement;
            modifiedElement.Delete(2);
            modifiedElement[2].Value.Should().Be(element[3].Value);
            modifiedElement[3].Value.Should().Be(element[4].Value);
        }
    }
}