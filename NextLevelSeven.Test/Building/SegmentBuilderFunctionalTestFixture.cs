﻿using System.Linq;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Test.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class SegmentBuilderFunctionalTestFixture : DescendantElementBuilderBaseTestFixture<ISegmentBuilder, ISegment>
    {
        protected override ISegmentBuilder BuildBuilder()
        {
            return Message.Build(ExampleMessageRepository.Standard)[2];
        }

        [Test]
        public void SegmentBuilder_CanBuildSubcomponentsViaParams()
        {
            var builder = Message.Build(Any.Message());
            var segment = builder[builder.NextIndex];
            var val0 = Any.StringCaps(3);
            var val1 = Any.String();
            var val2 = Any.String();
            segment.Type = val0;
            segment.SetSubcomponents(1, 1, 1, val1, val2);
            segment.RawValue.Should().Be($"{val0}|{val1}&{val2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildComponentsViaParams()
        {
            var builder = Message.Build(Any.Message());
            var segment = builder[builder.NextIndex];
            var val0 = Any.StringCaps(3);
            var val1 = Any.String();
            var val2 = Any.String();
            segment.Type = val0;
            segment.SetComponents(1, 1, 2, val1, val2);
            segment.RawValue.Should().Be($"{val0}|^{val1}^{val2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildRepetitionsViaParams()
        {
            var builder = Message.Build(Any.Message());
            var segment = builder[builder.NextIndex];
            var val0 = Any.StringCaps(3);
            var val1 = Any.String();
            var val2 = Any.String();
            segment.Type = val0;
            segment.SetFieldRepetitions(1, 2, val1, val2);
            segment.RawValue.Should().Be($"{val0}|~{val1}~{val2}");
        }

        [Test]
        [TestCase(3, -1)]
        [TestCase(-1, 3)]
        [TestCase(-2, -4)]
        public void SegmentBuilder_ThrowsOnNegativeIndexMove(int from, int to)
        {
            var segment = Message.Build(Any.Message())[1];
            segment.Invoking(s => s.Move(from, to)).Should().Throw<ElementException>();
        }

        [Test]
        public void SegmentBuilder_HasCorrectNextIndex()
        {
            var message = Message.Build(Any.Message());
            var segment = message[1];
            segment.NextIndex.Should().Be(segment.Fields.Last().Index + 1);
        }

        [Test]
        public void SegmentBuilder_HasNoValueWhenSettingEmptyValues()
        {
            var message = Message.Build(Any.Message());
            var builder = message[message.NextIndex];
            builder.RawValues = Enumerable.Empty<string>();
            builder.RawValue.Should().BeNull();
        }

        [Test]
        public void SegmentBuilder_NewSegmentHasNullValue()
        {
            var message = Message.Build(Any.Message());
            var builder = message[message.NextIndex];
            builder.RawValue.Should().BeNull();
        }

        [Test]
        public void SegmentBuilder_NewSegmentHasNoValues()
        {
            var message = Message.Build(Any.Message());
            var builder = message[message.NextIndex];
            builder.RawValues.Should().BeEmpty();
        }

        [Test]
        public void SegmentBuilder_HasDelimiter()
        {
            var builder = Message.Build(Any.Message());
            builder[1].Delimiter.Should().Be('|');
        }

        [Test]
        public void SegmentBuilder_CanMoveFields()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard);
            var val0 = Any.String();
            builder[1][3].RawValue = val0;
            builder[1][3].Move(4);
            builder[1][4].RawValue.Should().Be(val0);
        }

        [Test]
        [TestCase("")]
        [TestCase("M")]
        [TestCase("MS")]
        public void SegmentBuilder_Throws_WhenDataIsTooShort(string value)
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1];
            builder.Invoking(b => b.RawValue = value).Should().Throw<ElementException>();
        }

        [Test]
        public void SegmentBuilder_Throws_WhenMshDataIsNull()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1];
            builder.Invoking(b => b.RawValue = null).Should().Throw<ElementException>();
        }

        [Test]
        public void SegmentBuilder_CanNullifyNonMshSegment()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[2];
            builder.RawValue = null;
        }

        [Test]
        public void SegmentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1];
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SegmentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1] as ISegment;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SegmentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1] as IElement;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SegmentBuilder_CanGetField()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1][3].RawValue.Should().Be(builder.Segment(1).Field(3).RawValue)
                .And.NotBeNull();
        }

        [Test]
        public void SegmentBuilder_CanGetFields()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety)[1];
            // +1 is added because this is an MSH segment
            builder.Fields.Count().Should().Be(builder.RawValue.Split('|').Length + 1);
        }

        [Test]
        public void SegmentBuilder_CanGetValue()
        {
            var builder = Message.Build("MSH|^~\\&\rPID|1234")[1];
            builder.RawValue.Should().Be("MSH|^~\\&");
        }

        [Test]
        public void SegmentBuilder_CanGetValues()
        {
            var builder = Message.Build("MSH|^~\\&")[1];
            builder.RawValues.Should().Equal("MSH", "|", "^~\\&");
        }

        [Test]
        public void SegmentBuilder_CanBuildFields_Individually()
        {
            var builder = Message.Build()[1];
            var field3 = Any.String();
            var field5 = Any.String();

            builder
                .SetField(3, field3)
                .SetField(5, field5);
            builder.RawValue.Should().Be($"MSH|^~\\&|{field3}||{field5}");
        }

        [Test]
        public void SegmentBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var field3 = Any.String();
            var field5 = Any.String();

            builder
                .SetField(5, field5)
                .SetField(3, field3);
            builder.RawValue.Should().Be($"MSH|^~\\&|{field3}||{field5}");
        }

        [Test]
        public void SegmentBuilder_CanBuildFields_Sequentially()
        {
            var builder = Message.Build()[1];
            var field3 = Any.String();
            var field5 = Any.String();

            builder
                .SetFields(3, field3, null, field5);
            builder.RawValue.Should().Be($"MSH|^~\\&|{field3}||{field5}");
        }

        [Test]
        public void SegmentBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build()[1];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetition(3, 1, repetition1)
                .SetFieldRepetition(3, 2, repetition2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{repetition1}~{repetition2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetition(3, 2, repetition2)
                .SetFieldRepetition(3, 1, repetition1);
            builder.RawValue.Should().Be($"MSH|^~\\&|{repetition1}~{repetition2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetitions(3, repetition1, repetition2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{repetition1}~{repetition2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(3, 1, 1, component1)
                .SetComponent(3, 1, 2, component2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{component1}^{component2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(3, 1, 2, component2)
                .SetComponent(3, 1, 1, component1);
            builder.RawValue.Should().Be($"MSH|^~\\&|{component1}^{component2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponents(3, 1, component1, component2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{component1}^{component2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(3, 1, 1, 1, subcomponent1)
                .SetSubcomponent(3, 1, 1, 2, subcomponent2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(3, 1, 1, 2, subcomponent2)
                .SetSubcomponent(3, 1, 1, 1, subcomponent1);
            builder.RawValue.Should().Be($"MSH|^~\\&|{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void SegmentBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponents(3, 1, 1, 1, subcomponent1, subcomponent2);
            builder.RawValue.Should().Be($"MSH|^~\\&|{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void SegmentBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1];
            builder.Encoding.FieldDelimiter.Should().Be('|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            builder.Encoding.FieldDelimiter.Should().Be(':');
        }

        [Test]
        public void SegmentBuilder_ChangesEncodingCharactersIfMshSegmentChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1];
            builder.Encoding.FieldDelimiter.Should().Be('|');
            builder.SetField(1, ":");
            builder.Encoding.FieldDelimiter.Should().Be(':');
        }

        [Test]
        public void SegmentBuilder_CanGetType()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[2];
            var type = Any.StringCaps(3);
            builder[0].RawValue = type;
            builder.Type.Should().Be(type);
        }

        [Test]
        public void SegmentBuilder_CanSetType()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[2];
            var type = Any.StringCaps(3);
            builder.Type = type;
            builder[0].RawValue.Should().Be(type);
        }
    }
}