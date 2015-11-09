using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class RepetitionParserFunctionalTests : ParsingTestFixture
    {
        [Test]
        public void Repetition_HasFieldAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1];
            Assert.IsNotNull(element.Ancestor);
        }

        [Test]
        public void Repetition_HasGenericFieldAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1] as IRepetition;
            Assert.IsNotNull(element.Ancestor);
        }

        [Test]
        public void Repetition_HasGenericAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1] as IElementParser;
            Assert.IsNotNull(element.Ancestor as IField);
        }

        [Test]
        public void Repetition_CanMoveComponents()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1];
            element.Values = new[] {MockFactory.String(), MockFactory.String(), MockFactory.String(), MockFactory.String()};
            var newMessage = element.Clone();
            newMessage[2].Move(3);
            Assert.AreEqual(element[2].Value, newMessage[3].Value);
        }

        [Test]
        public void Repetition_Throws_WhenIndexedBelowOne()
        {
            var element = Message.Parse(ExampleMessages.Standard)[1][3][1];
            string value = null;
            AssertAction.Throws<ParserException>(() => { value = element[0].Value; });
            Assert.IsNull(value);
        }

        [Test]
        public void Repetition_CanBeCloned()
        {
            var repetition = Message.Parse(ExampleMessages.Standard)[1][3][1];
            var clone = repetition.Clone();
            Assert.AreNotSame(repetition, clone, "Cloned repetition is the same referenced object.");
            Assert.AreEqual(repetition.Value, clone.Value, "Cloned repetition has different contents.");
        }

        [Test]
        public void Repetition_CanBeClonedGenerically()
        {
            IElement repetition = Message.Parse(ExampleMessages.Standard)[1][3][1];
            var clone = repetition.Clone();
            Assert.AreNotSame(repetition, clone, "Cloned repetition is the same referenced object.");
            Assert.AreEqual(repetition.Value, clone.Value, "Cloned repetition has different contents.");
        }

        [Test]
        public void Repetition_CanAddDescendantsAtEnd()
        {
            var repetition = Message.Parse(ExampleMessages.Standard)[2][3][4];
            var count = repetition.ValueCount;
            var id = MockFactory.String();
            repetition[count + 1].Value = id;
            Assert.AreEqual(count + 1, repetition.ValueCount,
                @"Number of elements after appending at the end of a repetition is incorrect.");
        }

        [Test]
        public void Repetition_CanGetComponents()
        {
            var repetition = Message.Parse(ExampleMessages.Standard)[8][13][2];
            Assert.AreEqual(3, repetition.Components.Count());
        }

        [Test]
        public void Repetition_CanGetComponentsGenerically()
        {
            IRepetition repetition = Message.Parse(ExampleMessages.Standard)[8][13][2];
            Assert.AreEqual(3, repetition.Components.Count());
        }

        [Test]
        public void Repetition_CanGetComponentsByIndexer()
        {
            var component = Message.Parse(ExampleMessages.Standard)[8][13][2][2];
            Assert.AreEqual(@"ORN", component.Value);
        }

        [Test]
        public void Repetition_CanDeleteComponent()
        {
            var message = Message.Parse("MSH|^~\\&|\rTST|123^456~789^012");
            var component = message[2][1][2];
            ElementExtensions.Delete(component, 1);
            Assert.AreEqual("MSH|^~\\&|\rTST|123^456~012", message.Value, @"Message was modified unexpectedly.");
        }

        [Test]
        public void Repetition_CanWriteStringValue()
        {
            var repetition = Message.Parse(ExampleMessages.Standard)[1][3][1];
            var value = MockFactory.String();
            repetition.Value = value;
            Assert.AreEqual(value, repetition.Value, "Value mismatch after write.");
        }

        [Test]
        public void Repetition_CanWriteNullValue()
        {
            var repetition = Message.Parse(ExampleMessages.Standard)[1][3][1];
            var value = MockFactory.String();
            repetition.Value = value;
            repetition.Value = null;
            Assert.IsNull(repetition.Value, "Value mismatch after write.");
        }
    }
}