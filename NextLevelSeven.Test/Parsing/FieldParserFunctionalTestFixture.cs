using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class FieldParserFunctionalTestFixture : DescendantElementParserBaseTestFixture<IFieldParser, IField>
    {
        protected override IFieldParser BuildParser()
        {
            return Message.Parse(ExampleMessageRepository.Standard)[1][3];
        }

        [Test]
        public void Field_Delimiter_CanBeCloned()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            var clone = field.Clone();
            clone.Should().NotBeSameAs(field);
            clone.Value.Should().Be(field.Value);
        }

        [Test]
        public void Field_Delimiter_CanGetValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            field.Value.Should().Be("|");
        }

        [Test]
        public void Field_Delimiter_CanGetNullSegmentValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[2][1];
            field.Value.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Delimiter_CanNotSetNullSegmentValue()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var field = message[1][1];
            field.Value = null;
        }

        [Test]
        public void Field_Encoding_CanBeCloned()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][2];
            var clone = field.Clone();
            clone.Should().NotBeSameAs(field);
            clone.Value.Should().Be(field.Value);
        }

        [Test]
        public void Field_Encoding_CanGetValues()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][2];
            field.Values.Should().Equal("^", "~", "\\", "&");
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Encoding_CanNotSetValues()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][2];
            var values = new[] {"^", "~", "\\", "&"};
            field.Values = values;
        }

        [Test]
        public void Field_Encoding_HasNoDelimiter()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][2];
            field.Delimiter.Should().Be('\0');
        }

        [Test]
        public void Field_Encoding_HasNoDescendants()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][2];
            field.Descendants.Should().BeEmpty();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Encoding_ThrowsOnDescendantAccess()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][2];
            field[2].Value.Should().BeNull();
        }

        [Test]
        public void Field_Delimiter_HasNoDelimiter()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            field.Delimiter.Should().Be('\0');
        }

        [Test]
        public void Field_Delimiter_HasNoDescendants()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            field.Descendants.Should().BeEmpty();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Delimiter_CanNotChangeValues()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            var values = new[] {"$", "@"};
            field.Values = values;
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Delimiter_ThrowsOnDescendantAccess()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            field[2].Value.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Delimiter_ThrowsOnGenericDescendantAccess()
        {
            IElementParser field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            field[2].Should().BeNull();
        }

        [Test]
        public void Field_Delimiter_HasOneValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][1];
            field.ValueCount.Should().Be(1);
            field.Value.Should().Be(field.GetValue());
            field.Values.Count().Should().Be(1);
            field.GetValues().Count().Should().Be(1);
        }

        [Test]
        public void Field_CanGetAncestor()
        {
            var segment = Message.Parse(ExampleMessageRepository.Standard)[1];
            var field = segment[3];
            field.Ancestor.Should().Be(segment);
        }

        [Test]
        public void Field_CanGetIsolatedValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][3];
            var val0 = Any.String();
            field.Value = val0;
            var newField = field.Clone();
            newField.Value.Should().Be(field.Value);
            field.Values.Count().Should().Be(1);
        }

        [Test]
        public void Field_CanGetIsolatedNullValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Minimum)[1][3];
            var newField = field.Clone();
            newField.Value.Should().BeNull();
        }

        [Test]
        public void Field_CanInsertRepetitions()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3];
            element.Values = new[] { Any.String(), Any.String(), Any.String(), Any.String() };
            var value = Any.String();
            element.Insert(1, value);
            element.ValueCount.Should().Be(5);
            element[1].Value.Should().Be(value);
        }

        [Test]
        public void Field_CanInsertElementRepetitions()
        {
            var otherElement = Message.Parse(Any.Message())[1][3][1];
            var element = Message.Parse(Any.Message())[1][3];
            element.Values = new[] { Any.String(), Any.String(), Any.String(), Any.String() };
            element.Insert(1, otherElement);
            element.ValueCount.Should().Be(5);
            element[1].Value.Should().Be(otherElement.Value);
        }

        [Test]
        public void Field_CanMoveRepetitions()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3];
            element.Values = new[] {Any.String(), Any.String(), Any.String(), Any.String()};
            var newMessage = element.Clone();
            newMessage[2].Move(3);
            newMessage[3].Value.Should().Be(element[2].Value);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Field_Throws_WhenIndexedBelowOne()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[1][3];
            element[0].Value.Should().BeNull();
        }

        [Test]
        public void Field_HasCorrectIndex()
        {
            var field = Message.Parse(ExampleMessageRepository.Standard)[1][3];
            field.Index.Should().Be(3);
        }

        [Test]
        public void Field_HasCorrectIndex_ByExtension()
        {
            var field = Message.Parse(ExampleMessageRepository.Standard)[1].Field(3);
            field.Index.Should().Be(3);
        }

        [Test]
        public void Field_CanBeErased()
        {
            var field = (IElement)Message.Parse(ExampleMessageRepository.Standard)[1][3];
            field.Erase();
            field.Value.Should().BeNull();
        }

        [Test]
        public void Field_CanAddDescendantsAtEnd()
        {
            var field = Message.Parse(ExampleMessageRepository.Standard)[2][3];
            var count = field.ValueCount;
            var id = Any.String();
            field[count + 1].Value = id;
            field.ValueCount.Should().Be(count + 1);
        }

        [Test]
        public void Field_CanGetRepetitions()
        {
            var repetitions = Message.Parse(ExampleMessageRepository.Standard)[8][13].Repetitions;
            repetitions.Count().Should().Be(2);
        }

        [Test]
        public void Field_CanGetRepetitionsGenerically()
        {
            var repetitions = (Message.Parse(ExampleMessageRepository.Standard)[8][13] as IField).Repetitions;
            repetitions.Count().Should().Be(2);
        }

        [Test]
        public void Field_CanGetRepetitionsByIndexer()
        {
            var repetition = Message.Parse(ExampleMessageRepository.Standard)[8][13][2];
            repetition.Value.Should().Be("^ORN^CP");
        }

        [Test]
        public void Field_CanDeleteRepetition()
        {
            var message = Message.Parse("MSH|^~\\&|\rTST|123~456|789~012");
            var field = message[2][1];
            ElementExtensions.Delete(field, 1);
            message.Value.Should().Be("MSH|^~\\&|\rTST|456|789~012");
        }

        [Test]
        public void Field_WillPointToCorrectValue_WhenOtherFieldChanges()
        {
            var message = Message.Parse(string.Format(@"MSH|^~\&|{0}|{1}", Any.String(), Any.String()));
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var newMsh3Value = Any.String();
            var newMsh4Value = Any.String();

            message.Value = string.Format(@"MSH|^~\&|{0}|{1}", newMsh3Value, newMsh4Value);
            msh3.Value.Should().Be(newMsh3Value);
            msh4.Value.Should().Be(newMsh4Value);
        }

        [Test]
        public void Field_WithNoSignificantDescendants_ShouldNotClaimToHaveSignificantDescendants()
        {
            var message = Message.Parse();
            message[1][3].HasSignificantDescendants().Should().BeFalse();
        }

        [Test]
        public void Field_WillHaveValuesInterpretedAsDoubleQuotes()
        {
            var message = Message.Parse();
            message[1][3].Value = HL7.Null;
            message[1][3].Value.Should().Be(HL7.Null);
            message[1][3].Exists.Should().BeTrue();
        }

        [Test]
        public void Field_CanWriteStringValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Standard)[1][3];
            var value = Any.String();
            field.Value = value;
            field.Value.Should().Be(value);
        }

        [Test]
        public void Field_CanWriteNullValue()
        {
            var field = Message.Parse(ExampleMessageRepository.Standard)[1][3];
            var value = Any.String();
            field.Value = value;
            field.Value = null;
            field.Value.Should().BeNull();
        }
    }
}