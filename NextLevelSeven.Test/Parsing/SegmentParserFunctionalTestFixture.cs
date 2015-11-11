using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class SegmentParserFunctionalTestFixture : DescendantElementParserBaseTestFixture<ISegmentParser, ISegment>
    {
        protected override ISegmentParser BuildParser()
        {
            return Message.Parse(ExampleMessageRepository.Standard)[2];
        }

        [Test]
        public void Segment_CloneCanGetDelimiter()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[2].Clone();
            element.Delimiter.Should().Be('|');
        }

        [Test]
        public void Segment_CanGetAndSetType()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[2];
            var newType = Any.StringCaps(3);
            element.Type = newType;
            element.Type.Should().Be(newType);
            element[0].Value.Should().Be(newType);
        }

        [Test]
        public void Segment_CanMoveMshFields()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1];
            element[3].Value = Any.String();
            element[4].Value = Any.String();
            element[5].Value = Any.String();
            var newMessage = element.Clone();
            newMessage[3].Move(4);
            newMessage[4].Value.Should().Be(element[3].Value);
        }

        [Test]
        public void Segment_CanMoveFields()
        {
            var element = Message.Parse(Any.Message())[2];
            element[3].Value = Any.String();
            element[4].Value = Any.String();
            element[5].Value = Any.String();
            var newMessage = element.Clone();
            newMessage[3].Move(4);
            newMessage[4].Value.Should().Be(element[3].Value);
        }

        [Test]
        public void Segment_CanSetValues()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var type = Any.StringCaps(3);
            var data = Any.String();
            message[2].Values = new[] { type, data };
            message[2].Value.Should().Be(string.Format("{0}|{1}", type, data));
        }

        [Test]
        public void Segment_CanSetValuesOnMsh()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var data = Any.String();
            const string delimiter = "$";
            message[1].Values = new[] { "MSH", delimiter, data };
            message[1].Value.Should().Be(string.Format("MSH{0}{1}", delimiter, data));
        }

        [Test]
        public void Segment_CanInsertFields()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[2];
            var data = Any.String();
            segment.Insert(1, data);
            segment[1].Value.Should().Be(data);
        }

        [Test]
        public void Segment_CanInsertFieldElement()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[2];
            var data = Message.Parse(Any.Message())[2][1];
            segment.Insert(1, data);
            segment[1].Value.Should().Be(data.Value);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Segment_CanNotInsertMshElement()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[1];
            var data = Message.Parse(Any.Message())[2][1];
            segment.Insert(1, data);
        }

        [Test]
        public void Segment_GetsNextIndex()
        {
            var message = Message.Parse(Any.Message());
            var segment = message[2];
            segment.NextIndex.Should().Be(segment.ValueCount);
        }

        [Test]
        public void Segment_CanInsertFieldsInMsh()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[1];
            var data = Any.String();
            segment.Insert(3, data);
            segment[3].Value.Should().Be(data);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Segment_Throws_WhenIndexedBelowZero()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[1];
            element[-1].Value.Should().BeNull();
        }

        [Test]
        public void Segment_WithIdenticalValueToAnotherSegment_IsEquivalent()
        {
            var segment1 = Message.Parse(ExampleMessageRepository.Standard)[1];
            var segment2 = Message.Parse(ExampleMessageRepository.Standard)[1];
            segment2.Value.Should().Be(segment1.Value);
        }

        [Test]
        public void Segment_ReportsCorrectType()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message[1].Type.Should().Be("MSH");
        }

        [Test]
        public void Segment_HasMessageAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1];
            element.Ancestor.Should().NotBeNull();
        }

        [Test]
        public void Segment_HasGenericMessageAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1] as ISegment;
            element.Ancestor.Should().NotBeNull();
        }

        [Test]
        public void Segment_HasGenericAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1] as IElementParser;
            (element.Ancestor as IMessage).Should().NotBeNull();
        }

        [Test]
        public void Segment_HasClonedEncoding()
        {
            var segment = Message.Parse(ExampleMessageRepository.Standard)[1];
            var clone = segment.Clone();
            clone.Encoding.Should().Be(segment.Encoding);
        }

        [Test]
        public void Segment_CanAddDescendantsAtEnd()
        {
            var segment = Message.Parse(ExampleMessageRepository.Standard)[2];
            var fieldCount = segment.ValueCount;
            var id = Any.String();
            segment[fieldCount].Value = id;
            segment.ValueCount.Should().Be(fieldCount + 1);
        }

        [Test]
        public void Segment_CanGetFields()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[2];
            segment.Fields.Count().Should().Be(7);
        }

        [Test]
        public void Segment_CanGetFieldsGenerically()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            ISegment segment = message[2];
            segment.Fields.Count().Should().Be(7);
        }

        [Test]
        public void Segment_CanGetFieldsByIndexer()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[2];
            var field = segment[2];
            field.Value.Should().Be("20130528073829");
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Segment_CanDeleteField(int segmentIndex)
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[segmentIndex];
            var field3 = segment[3].Value;
            var field5 = segment[5].Value;
            var field6 = segment[6].Value;
            ElementExtensions.Delete(segment, 4);
            segment[3].Value.Should().Be(field3);
            segment[4].Value.Should().Be(field5);
            segment[5].Value.Should().Be(field6);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [ExpectedException(typeof(ElementException))]
        public void Segment_CanNotDeleteMshEncodingFields(int index)
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[1];
            ElementExtensions.Delete(segment, index);
        }

        [Test]
        public void Segment_CanDeleteFieldsViaLinq()
        {
            var message = Message.Parse("MSH|^~\\&|1|2|3|4|5");
            var segment = message[1];
            segment.Descendants.Skip(2).Where(i => i.Converter.AsInt%2 == 0).Delete();
            message.Value.Should().Be("MSH|^~\\&|1|3|5");
        }

        [Test]
        public void Segment_WillPointToCorrectFieldValue_WhenFieldsChange()
        {
            var message = Message.Parse();
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var expected = Any.String().Substring(0, 5);

            msh4.Value = expected;
            msh3.Value = Any.String();
            msh4.Value.Should().Be(expected);
        }

        [Test]
        public void Segment_WithSignificantDescendants_ShouldClaimToHaveSignificantDescendants()
        {
            var message = Message.Parse();
            message[1].HasSignificantDescendants().Should().BeTrue();
        }

        [Test]
        public void Segment_WillConsiderNonPresentValuesToNotExist()
        {
            var message = Message.Parse();
            message[2].Exists.Should().BeFalse();
        }

        [Test]
        public void Segment_CanWriteStringValue()
        {
            var segment = Message.Parse(ExampleMessageRepository.Standard)[2];
            var value = Any.String();
            segment.Value = value;
            segment.Value.Should().Be(value);
        }

        [Test]
        public void Segment_CanWriteNullValue()
        {
            var segment = Message.Parse(ExampleMessageRepository.Standard)[2];
            var value = Any.String();
            segment.Value = value;
            segment.Value = null;
            segment.Value.Should().BeNull();
        }
    }
}