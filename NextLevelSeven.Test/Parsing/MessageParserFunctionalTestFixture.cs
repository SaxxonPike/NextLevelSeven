﻿using System;
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
    public class MessageParserFunctionalTestFixture : ElementParserBaseTestFixture<IMessageParser, IMessage>
    {
        protected override IMessageParser BuildParser()
        {
            return Message.Parse(ExampleMessageRepository.Standard);
        }

        [Test]
        public void Message_CanProcessMessageWithShortEncoding()
        {
            var field31 = Any.String();
            var field32 = Any.String();
            var field41 = Any.String();
            var field42 = Any.String();
            var message = Message.Parse($"MSH|^|{field31}^{field32}|{field41}^{field42}");
            message[1][3][1][1].RawValue.Should().Be(field31);
            message[1][3][1][2].RawValue.Should().Be(field32);
            message[1][4][1][1].RawValue.Should().Be(field41);
            message[1][4][1][2].RawValue.Should().Be(field42);
        }

        [Test]
        public void Message_IsEquivalentWhenReferencesMatch()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var generic = (object)message;
            message.Should().Be(generic);
        }

        [Test]
        public void Message_IsNotEquivalentWhenNull()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Should().NotBe(null);
        }

        [Test]
        public void Message_ToStringGetsValue()
        {
            var content = Any.Message();
            var message = Message.Parse(content);
            message.ToString().Should().Be(content);
        }

        [Test]
        public void Message_ToStringGetsEmptyWhenFieldIsNull()
        {
            var content = Any.Message();
            var field = Message.Parse(content)[1][3];
            field.RawValue = null;
            field.ToString().Should().BeEmpty();
        }

        [Test]
        public void Message_Validates()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.Validate().Should().BeTrue();
        }

        [Test]
        public void Message_DeletesOutOfRangeIndex()
        {
            var content = ExampleMessageRepository.Minimum;
            var message = Message.Parse(content);
            message.Delete(2);
            message.RawValue.Should().Be(content);
        }

        [Test]
        public void Message_InsertsElement()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var segment = Message.Parse(Any.Message())[2];
            message.Insert(2, segment);
            message[2].RawValue.Should().Be(segment.RawValue);
        }

        [Test]
        public void Message_InsertsString()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var segment = Any.Segment();
            message.Insert(2, segment);
            message[2].RawValue.Should().Be(segment);
        }

        [Test]
        public void Message_ThrowsOnInsertingNegativeIndex()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.Invoking(m => m.Insert(-2, Any.String())).Should().Throw<ElementException>();
        }

        [Test]
        public void Message_ThrowsOnDeletingNegativeIndex()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.Invoking(m => m.Delete(-2)).Should().Throw<ElementException>();
        }

        [Test]
        public void Message_DeletesZeroLengthItem()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum + "\r\r");
            message.Delete(2);
        }

        [Test]
        public void Message_ConvertsToBuilder()
        {
            var builder = Message.Parse(ExampleMessageRepository.Standard);
            var beforeMessageString = builder.RawValue;
            var message = builder.ToBuilder();
            message.RawValue.Should().Be(beforeMessageString);
        }

        [Test]
        public void Message_ConvertsFromBuilder()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var beforeBuilderString = message.RawValue;
            var afterBuilder = Message.Parse(message);
            afterBuilder.RawValue.Should().Be(beforeBuilderString);
        }

        [Test]
        public void Message_ThrowsWithIncorrectFirstSegment()
        {
            Action act = () => Message.Parse(Any.String());
            act.Should().Throw<ElementException>();
        }

        [Test]
        public void Message_CanGetValues()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.RawValues.First().Should().Be(ExampleMessageRepository.Minimum);
        }

        [Test]
        public void Message_CanSetValues()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.RawValues = new[] {ExampleMessageRepository.Minimum, ExampleMessageRepository.Minimum, ExampleMessageRepository.Minimum};
            message.ValueCount.Should().Be(3);
        }

        [Test]
        public void Message_CanIndexPastEnd()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message[5].RawValue.Should().BeNull();
        }

        [Test]
        public void Message_CanMoveSegments()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var newMessage = message.Clone();
            newMessage[2].Move(3);
            newMessage[3].RawValue.Should().Be(message[2].RawValue);
        }

        [Test]
        public void Message_Throws_WhenIndexedBelowOne()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard);
            element.Invoking(e => e[0].RawValue.Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void Message_CanNotSetMsh1()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.Invoking(m => m[1][1].RawValue = ":").Should().Throw<ElementException>();
        }

        [Test]
        public void Message_CanNotSetMsh2()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            message.Invoking(m => m[1][2].RawValue = "!@#$").Should().Throw<ElementException>();
        }

        [Test]
        public void Message_ConvertsMshCorrectly()
        {
            var message = Message.Parse(ExampleMessageRepository.MshOnly);
            message.RawValue.Should().Be(ExampleMessageRepository.MshOnly);
        }

        [Test]
        public void Message_ReturnsBasicMessage()
        {
            var message = Message.Parse();
            message.ValueCount.Should().Be(1);
            message[1].Type.Should().Be("MSH");
            message[1][2].RawValue.Should().Be(@"^~\&");
            message[1][1].RawValue.Should().Be("|");
        }

        [Test]
        public void Message_ThrowsOnNullData()
        {
            Action act = () => Message.Parse((string) null);
            act.Should().Throw<ElementException>();
        }

        [Test]
        public void Message_ThrowsOnEmptyData()
        {
            Action act = () => Message.Parse(string.Empty);
            act.Should().Throw<ElementException>();
        }

        [Test]
        public void Message_ThrowsOnShortData()
        {
            Action act = () => Message.Parse("MSH|123");
            act.Should().Throw<ElementException>();
        }

        [Test]
        public void Message_CanRetrieveMessageTypeAndTriggerEvent()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Details.Type.Should().Be("ADT");
            message.Details.TriggerEvent.Should().Be("A17");
        }

        [Test]
        public void Message_CanParseMessageDateTime()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Details.Time.Should().Be(new DateTime(2013, 05, 28, 07, 38, 29));
        }

        [Test]
        public void Message_CanRetrieveMessageVersion()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Details.Version.Should().Be("2.3");
        }

        [Test]
        public void Message_CanRetrievePatientId()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var pid = message.Segments.OfType("PID").First();
            pid[5][1][1].RawValue.Should().Be("Colon");
        }

        [Test]
        public void Message_CanRetrieveMultipleSegments()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.Segments.OfType("OBX").Count().Should().Be(3);
        }

        [Test]
        public void Message_CanRetrieveRepetitions()
        {
            var message = Message.Parse(ExampleMessageRepository.RepeatingName);
            var pid = message.Segments.OfType("PID").First();
            pid[5][1].RawValue.Should().Be("Lincoln^Abe");
            pid[5][2].RawValue.Should().Be("Bro");
            pid[5][3].RawValue.Should().Be("Dude");
        }

        [Test]
        public void Message_RetrievalMethodsAreIdentical()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.GetValue(1).Should().Be(message[1].RawValue);
            message.GetValue(1, 3).Should().Be(message[1][3].RawValue);
            message.GetValue(1, 3, 1).Should().Be(message[1][3][1].RawValue);
            message.GetValue(1, 3, 1, 1).Should().Be(message[1][3][1][1].RawValue);
            message.GetValue(1, 3, 1, 1, 1).Should().Be(message[1][3][1][1][1].RawValue);
        }

        [Test]
        public void Message_MultiRetrievalMethodsAreIdentical()
        {
            var message = Message.Parse(ExampleMessageRepository.Variety);
            message[1].RawValues.Should().Equal(message.GetValues(1));
            message[1][3].RawValues.Should().Equal(message.GetValues(1, 3));
            message[1][3][1].RawValues.Should().Equal(message.GetValues(1, 3, 1));
            message[1][3][1][1].RawValues.Should().Equal(message.GetValues(1, 3, 1, 1));
            message[1][3][1][1][1].RawValues.Should().Equal(message.GetValues(1, 3, 1, 1, 1));
        }

        [Test]
        public void Message_HasUniqueDescendantKeys()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var keys = message.Segments.Select(s => s.Key).ToList();
            var distinctKeys = keys.Distinct();
            distinctKeys.Count().Should().Be(message.Segments.Count());
        }

        [Test]
        public void Message_WithOnlyOneSegment_WillClaimToHaveSignificantDescendants()
        {
            var message = Message.Parse();
            message.HasSignificantDescendants().Should().BeTrue();
        }

        [Test]
        public void Message_CanMapSegments()
        {
            var id = Any.String();
            var content = $"MSH|^~\\&|{id}";
            IMessage tree = Message.Parse(content);
            tree.GetValue(1).Should().Be(content);
        }

        [Test]
        public void Message_CanMapFields()
        {
            var id = Any.String();
            IMessage tree = Message.Parse($"MSH|^~\\&|{id}");
            tree.GetValue(1, 3).Should().Be(id);
        }

        [Test]
        public void Message_CanMapRepetitions()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            IMessage tree = Message.Parse($"MSH|^~\\&|{id1}~{id2}");
            tree.GetValue(1, 3, 1).Should().Be(id1);
            tree.GetValue(1, 3, 2).Should().Be(id2);
        }

        [Test]
        public void Message_CanMapComponents()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            IMessage tree = Message.Parse($"MSH|^~\\&|{id1}^{id2}");
            tree.GetValue(1, 3, 1, 1).Should().Be(id1);
            tree.GetValue(1, 3, 1, 2).Should().Be(id2);
        }

        [Test]
        public void Message_CanMapSubcomponents()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            IMessage tree = Message.Parse($"MSH|^~\\&|{id1}&{id2}");
            tree.GetValue(1, 3, 1, 1, 1).Should().Be(id1);
            tree.GetValue(1, 3, 1, 1, 2).Should().Be(id2);
        }

        [Test]
        public void Message_CanAddDescendantsAtEnd()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var nextIndex = message.NextIndex;
            var count = message.ValueCount;
            var id = Any.String();
            message[nextIndex].RawValue = id;
            message.ValueCount.Should().Be(count + 1);
        }

        [Test]
        public void Message_CanGetSegmentsByIndexer()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment = message[1];
            segment.RawValue.Should().Be(message.RawValues.First());
        }

        [Test]
        public void Message_CanDeleteSegment()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            var segment1 = message[1].RawValue;
            var segment3 = message[3].RawValue;
            var segment4 = message[4].RawValue;
            ElementExtensions.Delete(message, 2);
            message[1].RawValue.Should().Be(segment1);
            message[2].RawValue.Should().Be(segment3);
            message[3].RawValue.Should().Be(segment4);
        }

        [Test]
        public void Message_ValuesReturnsProperlySplitData()
        {
            var message = Message.Parse(ExampleMessageRepository.Standard);
            message.RawValues.Should().Equal(message.RawValue.Split('\xD'));
        }
    }
}