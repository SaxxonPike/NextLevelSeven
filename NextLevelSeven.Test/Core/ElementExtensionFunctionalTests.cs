using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class ElementExtensionFunctionalTests : CoreTestFixture
    {
        [Test]
        public void ElementExtensions_SimplifiesSegment()
        {
            var message = Message.Parse(MockFactory.Message());
            var segment = message.Segment(1);
            var fieldCount = segment.ValueCount;
            segment.Field(3).Nullify();
            Assert.AreEqual(fieldCount - 1, segment.Simplified().Count());
        }

        [Test]
        public void ElementExtensions_ThrowsWhenInsertingWithoutAncestor()
        {
            var message = Message.Parse();
            var segment = message[1].Clone();
            AssertAction.Throws<ElementException>(() => segment.Insert(MockFactory.String()));
        }

        [Test]
        public void ElementExtensions_ThrowsWhenInsertingElementWithoutHavingAncestor()
        {
            var message = Message.Parse(MockFactory.Message());
            var segment = message[1].Clone();
            AssertAction.Throws<ElementException>(() => segment.Insert(message[2]));
        }

        [Test]
        public void ElementExtensions_ThrowsWhenNullifyingMessage()
        {
            var message = Message.Parse(MockFactory.Message());
            AssertAction.Throws<ElementException>(message.Nullify);
        }

        [Test]
        public void ElementExtensions_NullifiesMshSegment()
        {
            var message = Message.Parse(MockFactory.Message());
            message[1].Nullify();
            Assert.AreEqual(3, message[1].ValueCount);
        }

        [Test]
        public void ElementExtensions_NullifiesSegment()
        {
            var message = Message.Parse(MockFactory.Message());
            message[2].Nullify();
            Assert.AreEqual(1, message[2].ValueCount);
        }

        [Test]
        public void ElementExtensions_FieldDelimiter_HasNoSignificantDescendants()
        {
            var message = Message.Parse(MockFactory.Message());
            Assert.IsFalse(message[1][1].HasSignificantDescendants());
        }

        [Test]
        public void ElementExtensions_EncodingCharacterField_HasNoSignificantDescendants()
        {
            var message = Message.Parse(MockFactory.Message());
            Assert.IsFalse(message[1][2].HasSignificantDescendants());
        }

        [Test]
        public void ElementExtensions_Element_AddsOtherElements()
        {
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            var message0 = Message.Build(ExampleMessages.Standard);
            var message1 = message0.Clone();
            message0[1][3].Value = val0;
            message1[1][3].Value = val1;
            message0.AddRange(message1.Segments.Skip(2));
            Assert.AreEqual(message0.ValueCount, message1.ValueCount*2 - 2);
            Assert.AreEqual(message0[message1.ValueCount + 1].Value, message1[3].Value);
        }

        [Test]
        public void ElementExtensions_Element_ThrowsWhenEncodingFieldIsMoved()
        {
            var message = Message.Build(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(() => message[1][2].Move(1));
        }

        [Test]
        public void ElementExtensions_Element_ThrowsWhenFieldDelimiterIsMoved()
        {
            var message = Message.Build(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(() => message[1][1].Move(2));
        }

        [Test]
        public void ElementExtensions_Element_ThrowsWhenSegmentTypeIsMoved()
        {
            var message = Message.Build(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(() => message[2][0].Move(2));
        }

        [Test]
        public void ElementExtensions_Element_DoesNotMoveWhenMovedToSameIndex()
        {
            var message = Message.Build(ExampleMessages.Standard);
            message[2].Move(2);
        }

        [Test]
        public void ElementExtensions_Builder_CanBeDeletedFromAncestor()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var newMessage = message.Clone();
            newMessage[2].Delete();
            Assert.AreEqual(message[3].Value, newMessage[2].Value);
        }

        [Test]
        public void ElementExtensions_Message_CannotBeDeletedFromAncestor()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(message.Delete);
        }

        [Test]
        public void ElementExtensions_Message_CannotBeMoved()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(() => message.Move(2));
        }

        [Test]
        public void ElementExtensions_Element_ThrowsWhenMoveIndexIsBelowMinimum()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(() => message[2].Move(-1));
        }

        [Test]
        public void ElementExtensions_Element_DoesNotThrowWhenDeletingZeroElements()
        {
            AssertAction.DoesNotThrow(Enumerable.Empty<IElement>().Delete);
        }

        [Test]
        public void ElementExtensions_Element_ThrowsWhenDeletingElementsFromDifferentAncestors()
        {
            var message0 = Message.Parse(ExampleMessages.Standard);
            var message1 = Message.Parse(ExampleMessages.Standard);
            AssertAction.Throws<ElementException>(() => new[] {message0[2], message1[2]}.Delete());
        }

        [Test]
        public void ElementExtensions_Parser_CanBeDeletedFromAncestor()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var newMessage = message.Clone();
            newMessage[2].Delete();
            Assert.AreEqual(message[3].Value, newMessage[2].Value);
        }

        [Test]
        public void ElementExtensions_CanCreateNewEmptyMessageFromZeroSegments()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var parser = message.Segments.OfType("|||").ToNewParser();
            Assert.AreEqual(1, parser.ValueCount);
            Assert.AreEqual("MSH", message[1].Type);
        }

        [Test]
        public void ElementExtensions_Parser_CanCreateNewMessageFromSegments()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var parser = message.Segments.OfType("PID").ToNewParser();
            Assert.AreEqual(3, parser.ValueCount);
            Assert.AreEqual(message[1].Value, parser[1].Value);
            Assert.AreEqual(message.Segments.OfType("PID").First().Value, parser[2].Value);
        }

        [Test]
        public void ElementExtensions_Parser_CanGetMultipleSegmentTypes()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var segments = message.Segments.OfTypes("MSH", "PID");
            Assert.AreEqual(3, segments.Count());
        }

        [Test]
        public void ElementExtensions_Parser_CanGetMultipleSegmentTypesAsEnumerable()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var segments = message.Segments.OfTypes(new [] {"MSH", "PID"}.AsEnumerable());
            Assert.AreEqual(3, segments.Count());
        }

        [Test]
        public void ElementExtensions_Parser_CanGetSegment()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1].Value);
            Assert.AreEqual(parser[1].Value, parser.Segment(1).Value, "Segments returned differ.");
        }

        [Test]
        public void ElementExtensions_Parser_CanGetField()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3].Value);
            Assert.AreEqual(parser[1][3].Value, parser.Segment(1).Field(3).Value, "Fields returned differ.");
        }

        [Test]
        public void ElementExtensions_Parser_CanGetRepetition()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][2].Value);
            Assert.AreEqual(parser[1][3][2].Value, parser.Segment(1).Field(3).Repetition(2).Value,
                "Repetitions returned differ.");
        }

        [Test]
        public void ElementExtensions_Parser_CanGetComponent()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][2][2].Value);
            Assert.AreEqual(parser[1][3][2][2].Value, parser.Segment(1).Field(3).Repetition(2).Component(2).Value,
                "Components returned differ.");
        }

        [Test]
        public void ElementExtensions_Parser_CanGetComponent_ThroughField()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][1][2].Value);
            Assert.AreEqual(parser[1][3][1][2].Value, parser.Segment(1).Field(3).Component(2).Value,
                "Components returned differ.");
        }

        [Test]
        public void ElementExtensions_Parser_CanGetSubcomponent()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][2][2][2].Value);
            Assert.AreEqual(parser[1][3][2][2][2].Value,
                parser.Segment(1).Field(3).Repetition(2).Component(2).Subcomponent(2).Value,
                "Subcomponents returned differ.");
        }

        // Delete Element

        private static void ElementExtensions_CanDelete(IElement element)
        {
            var modifiedElement = element.Clone();
            ElementExtensions.Delete(modifiedElement, 2);
            Assert.AreEqual(modifiedElement[2].Value, element[3].Value);
            Assert.AreEqual(modifiedElement[3].Value, element[4].Value);
        }

        [Test]
        public void ElementExtensions_Builder_CanDelete()
        {
            ElementExtensions_CanDelete(Message.Build(ExampleMessages.Standard));
        }

        [Test]
        public void ElementExtensions_Parser_CanDelete()
        {
            ElementExtensions_CanDelete(Message.Parse(ExampleMessages.Standard));
        }
    }
}