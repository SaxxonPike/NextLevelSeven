using System;
using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Test.Utility;
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
            element[0].RawValue.Should().Be(newType);
        }

        [Test]
        public void Segment_CanMoveMshFields()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1];
            element[3].RawValue = Any.String();
            element[4].RawValue = Any.String();
            element[5].RawValue = Any.String();
            var newMessage = element.Clone();
            newMessage[3].Move(4);
            newMessage[4].RawValue.Should().Be(element[3].RawValue);
        }

        [Test]
        public void Segment_CanMoveFields()
        {
            var element = Message.Parse(Any.Message())[2];
            element[3].RawValue = Any.String();
            element[4].RawValue = Any.String();
            element[5].RawValue = Any.String();
            var newMessage = element.Clone();
            newMessage[3].Move(4);
            newMessage[4].RawValue.Should().Be(element[3].RawValue);
        }

        [Test]
        public void Segment_CanSetValues()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var type = Any.StringCaps(3);
            var data = Any.String();
            message[2].RawValues = new[] { type, data };
            message[2].RawValue.Should().Be($"{type}|{data}");
        }

        [Test]
        public void Segment_CanSetValuesOnMsh()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var data = Any.String();
            const string delimiter = "$";
            message[1].RawValues = new[] { "MSH", delimiter, data };
            message[1].RawValue.Should().Be($"MSH{delimiter}{data}");
        }

        [Test]
        public void Segment_CanInsertFields()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[2];
            var data = Any.String();
            segment.Insert(1, data);
            segment[1].RawValue.Should().Be(data);
        }

        [Test]
        public void Segment_CanInsertFieldElement()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[2];
            var data = Message.Parse(Any.Message())[2][1];
            segment.Insert(1, data);
            segment[1].RawValue.Should().Be(data.RawValue);
        }

        [Test]
        public void Segment_CanNotInsertMshElement()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[1];
            var data = Message.Parse(Any.Message())[2][1];
            segment.Invoking(s => s.Insert(1, data)).Should().Throw<ElementException>();
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
            segment[3].RawValue.Should().Be(data);
        }

        [Test]
        public void Segment_Throws_WhenIndexedBelowZero()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[1];
            element.Invoking(e => e[-1].RawValue.Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void Segment_WithIdenticalValueToAnotherSegment_IsEquivalent()
        {
            var segment1 = Message.Parse(ExampleMessageRepository.Standard)[1];
            var segment2 = Message.Parse(ExampleMessageRepository.Standard)[1];
            segment2.RawValue.Should().Be(segment1.RawValue);
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
            segment[fieldCount].RawValue = id;
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
            field.RawValue.Should().Be("20130528073829");
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Segment_CanDeleteField(int segmentIndex)
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[segmentIndex];
            var field3 = segment[3].RawValue;
            var field5 = segment[5].RawValue;
            var field6 = segment[6].RawValue;
            ElementExtensions.Delete(segment, 4);
            segment[3].RawValue.Should().Be(field3);
            segment[4].RawValue.Should().Be(field5);
            segment[5].RawValue.Should().Be(field6);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Segment_CanNotDeleteMshEncodingFields(int index)
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[1];
            Action act = () => ElementExtensions.Delete(segment, index);
            act.Should().Throw<ElementException>();
        }

        [Test]
        public void Segment_CanDeleteFieldsViaLinq()
        {
            var message = Message.Parse("MSH|^~\\&|1|2|3|4|5");
            var segment = message[1];
            segment.Descendants.Skip(2).Where(i => i.As.Integer%2 == 0).Delete();
            message.RawValue.Should().Be("MSH|^~\\&|1|3|5");
        }

        [Test]
        public void Segment_WillPointToCorrectFieldValue_WhenFieldsChange()
        {
            var message = Message.Parse();
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var expected = Any.String().Substring(0, 5);

            msh4.RawValue = expected;
            msh3.RawValue = Any.String();
            msh4.RawValue.Should().Be(expected);
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
            segment.RawValue = value;
            segment.RawValue.Should().Be(value);
        }

        [Test]
        public void Segment_CanWriteNullValue()
        {
            var segment = Message.Parse(ExampleMessageRepository.Standard)[2];
            var value = Any.String();
            segment.RawValue = value;
            segment.RawValue = null;
            segment.RawValue.Should().BeNull();
        }
    }
}