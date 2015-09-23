using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ElementExtensionTests : CoreTestFixture
    {
        [TestMethod]
        public void ElementExtensions_Builder_CanCreateNewMessageFromSegments()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var builder = message.Segments.OfType("PID").ToNewBuilder();
            Assert.AreEqual(3, builder.ValueCount);
            Assert.AreEqual(message[1].Value, builder[1].Value);
            Assert.AreEqual(message.Segments.OfType("PID").First().Value, builder[2].Value);
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanCreateNewMessageFromSegments()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var parser = message.Segments.OfType("PID").ToNewParser();
            Assert.AreEqual(3, parser.ValueCount);
            Assert.AreEqual(message[1].Value, parser[1].Value);
            Assert.AreEqual(message.Segments.OfType("PID").First().Value, parser[2].Value);
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanGetSegment()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1].Value);
            Assert.AreEqual(builder[1].Value, builder.Segment(1).Value, "Segments returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanGetSegment()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1].Value);
            Assert.AreEqual(parser[1].Value, parser.Segment(1).Value, "Segments returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanGetField()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3].Value);
            Assert.AreEqual(builder[1][3].Value, builder.Segment(1).Field(3).Value, "Fields returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanGetField()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3].Value);
            Assert.AreEqual(parser[1][3].Value, parser.Segment(1).Field(3).Value, "Fields returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanGetRepetition()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][2].Value);
            Assert.AreEqual(builder[1][3][2].Value, builder.Segment(1).Field(3).Repetition(2).Value,
                "Repetitions returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanGetRepetition()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][2].Value);
            Assert.AreEqual(parser[1][3][2].Value, parser.Segment(1).Field(3).Repetition(2).Value,
                "Repetitions returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanGetComponent()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][2][2].Value);
            Assert.AreEqual(builder[1][3][2][2].Value, builder.Segment(1).Field(3).Repetition(2).Component(2).Value,
                "Components returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanGetComponent()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][2][2].Value);
            Assert.AreEqual(parser[1][3][2][2].Value, parser.Segment(1).Field(3).Repetition(2).Component(2).Value,
                "Components returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanGetComponent_ThroughField()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][1][2].Value);
            Assert.AreEqual(builder[1][3][1][2].Value, builder.Segment(1).Field(3).Component(2).Value,
                "Components returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanGetComponent_ThroughField()
        {
            var parser = Message.Parse(ExampleMessages.Variety);
            Assert.IsNotNull(parser[1][3][1][2].Value);
            Assert.AreEqual(parser[1][3][1][2].Value, parser.Segment(1).Field(3).Component(2).Value,
                "Components returned differ.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanGetSubcomponent()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            Assert.IsNotNull(builder[1][3][2][2][2].Value);
            Assert.AreEqual(builder[1][3][2][2][2].Value,
                builder.Segment(1).Field(3).Repetition(2).Component(2).Subcomponent(2).Value,
                "Subcomponents returned differ.");
        }

        [TestMethod]
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
            modifiedElement.Delete(2);
            Assert.AreEqual(modifiedElement[2].Value, element[3].Value);
            Assert.AreEqual(modifiedElement[3].Value, element[4].Value);
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanDelete()
        {
            ElementExtensions_CanDelete(Message.Build(ExampleMessages.Standard));
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanDelete()
        {
            ElementExtensions_CanDelete(Message.Parse(ExampleMessages.Standard));
        }

        // Insert Element After

        private static void ElementExtensions_CanInsertElementAfter(IElement element)
        {
            var modifiedElement = element.Clone();
            modifiedElement.InsertAfter(2, element[2]);
            Assert.AreEqual(modifiedElement[3].Value, element[2].Value,
                "Element insertion (after) failed: duplication didn't work.");
            Assert.AreEqual(modifiedElement[4].Value, element[3].Value,
                "Element insertion (after) failed: shifted segments weren't in order.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanInsertElementAfter()
        {
            ElementExtensions_CanInsertElementAfter(Message.Build(ExampleMessages.Standard));
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanInsertElementAfter()
        {
            ElementExtensions_CanInsertElementAfter(Message.Parse(ExampleMessages.Standard));
        }

        // Insert String After

        private static void ElementExtensions_CanInsertStringAfter(IElement element)
        {
            const string testSegment = @"TST|0|";

            var modifiedElement = element.Clone();
            modifiedElement[1].InsertAfter(testSegment);
            Assert.AreEqual(modifiedElement[2].Value, testSegment,
                "Text insertion (after) failed: expected string didn't appear in modified message.");
            Assert.AreEqual(modifiedElement[3].Value, element[2].Value,
                "Text insertion (after) failed: shifted segments weren't in order.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanInsertStringAfter()
        {
            ElementExtensions_CanInsertStringAfter(Message.Build(ExampleMessages.Standard));
        }

        [TestMethod]
        public void ElementExtensions_Parser_CanInsertStringAfter()
        {
            ElementExtensions_CanInsertStringAfter(Message.Parse(ExampleMessages.Standard));
        }
    }
}