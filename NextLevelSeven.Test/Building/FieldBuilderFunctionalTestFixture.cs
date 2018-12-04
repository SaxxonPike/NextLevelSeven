using System;
using System.Linq;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Test.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class FieldBuilderFunctionalTestFixture : DescendantElementBuilderBaseTestFixture<IFieldBuilder, IField>
    {
        protected override IFieldBuilder BuildBuilder()
        {
            return Message.Build(ExampleMessageRepository.Standard)[1][3];
        }

        [Test]
        public void FieldBuilder_Type_CannotMoveDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][0];
            builder.Invoking(b => b.Move(1, 2)).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Type_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][0];
            builder.Descendants.Count().Should().Be(0);
        }

        [Test]
        public void FieldBuilder_Type_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][0];
            builder.Delimiter.Should().Be('\0');
        }

        [Test]
        public void FieldBuilder_Type_ThrowsOnIndex()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][0];
            builder.Invoking(b => builder[1].RawValue.Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Type_SetsRepetitionOne()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[2][0];
            var value = Any.String();
            builder.SetFieldRepetition(1, value);
            builder.RawValue.Should().Be(value);
        }

        [Test]
        public void FieldBuilder_Type_ThrowsOnSettingRepetitionsOtherThanOne([Values(0, 2)] int index)
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[2][0];
            var value = Any.String();
            builder.Invoking(b => b.SetFieldRepetition(index, value)).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Encoding_Exists()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void FieldBuilder_Encoding_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Descendants.Count().Should().Be(0);
        }

        [Test]
        public void FieldBuilder_Encoding_HasMultipleValues()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.ValueCount.Should().Be(4);
        }

        [Test]
        public void FieldBuilder_Encoding_GetsValues()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.RawValue = "$%^&";
            builder.RawValues.Should().BeEquivalentTo("$", "%", "^", "&");
        }

        [Test]
        public void FieldBuilder_Encoding_SetsValue()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.RawValue = "$%^&";
            builder.RawValue.Should().Be("$%^&");
        }

        [Test]
        public void FieldBuilder_Encoding_SetsValues()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.RawValues = new[] {"$", "#", "~", "@"};
            builder.RawValue.Should().Be("$#~@");
        }

        [Test]
        public void FieldBuilder_Encoding_CanNullify()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Nullify();
            builder.RawValue.Should().BeNull();
        }

        [Test]
        public void FieldBuilder_Encoding_ThrowsOnSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Invoking(b => b[1].Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Encoding_ThrowsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Invoking(b => b.SetFieldRepetition(0, "~@#$").Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Encoding_ContainsCorrectDelimitersWithOversizedValue()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var builder = message[1][2];
            const string value = "!@#$%";
            builder.RawValue = value;
            message.Encoding.ComponentDelimiter.Should().Be('!');
            message.Encoding.RepetitionDelimiter.Should().Be('@');
            message.Encoding.EscapeCharacter.Should().Be('#');
            message.Encoding.SubcomponentDelimiter.Should().Be('$');
            builder.RawValue.Should().Be(value);
        }

        [Test]
        public void FieldBuilder_Encoding_SetsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.SetFieldRepetition(1, "~@#$");
            builder.RawValue.Should().Be("~@#$");
        }

        [Test]
        public void FieldBuilder_Encoding_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Delimiter.Should().Be('\0');
        }

        [Test]
        public void FieldBuilder_Delimiter_HasOneValue()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.ValueCount.Should().Be(1);
        }

        [Test]
        public void FieldBuilder_Delimiter_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Delimiter.Should().Be('\0');
        }

        [Test]
        public void FieldBuilder_Delimiter_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Descendants.Count().Should().Be(0);
        }

        [Test]
        public void FieldBuilder_Delimiter_GetsValue()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.RawValue.Should().Be("|");
        }

        [Test]
        public void FieldBuilder_Delimiter_GetsValues()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.RawValues.Count().Should().Be(1);
        }

        [Test]
        public void FieldBuilder_Delimiter_SetsSingleValueFromOnlyFirstCharacter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.RawValue = "$%^&";
            builder.RawValue.Should().Be("$");
        }

        [Test]
        public void FieldBuilder_Delimiter_SetsValuesFromOnlyFirstCharacter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.RawValues = new[] {"$", "#"};
            builder.RawValue.Should().Be("$");
        }

        [Test]
        public void FieldBuilder_Delimiter_CanNullify()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Nullify();
            builder.RawValue.Should().BeNull();
        }

        [Test]
        public void FieldBuilder_Delimiter_ThrowsOnSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Invoking(b => b[1].Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Delimiter_ThrowsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Invoking(b => b.SetFieldRepetition(0, "$")).Should().Throw<ElementException>();
        }

        [Test]
        public void FieldBuilder_Delimiter_SetsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.SetFieldRepetition(1, "$");
            builder.RawValue.Should().Be("$");
        }

        [Test]
        public void FieldBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void FieldBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1] as IField;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void FieldBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1] as IElement;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void FieldBuilder_CanGetRepetition()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1][3][2].RawValue.Should().NotBeNull();
            builder[1][3][2].RawValue.Should().Be(builder.Segment(1).Field(3).Repetition(2).RawValue);
        }

        [Test]
        public void FieldBuilder_CanGetRepetitions()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1][3].Repetitions.Count().Should().Be(builder[1][3].ValueCount);
        }

        [Test]
        public void FieldBuilder_CanGetValue()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{val0}~{val1}")[1][3];
            builder.RawValue.Should().Be($"{val0}~{val1}");
        }

        [Test]
        public void FieldBuilder_CanGetValues()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            var val3 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{val0}~{val1}^{val2}&{val3}")[1][3];
            builder.RawValues.Should().Equal(val0, $"{val1}^{val2}&{val3}");
        }

        [Test]
        public void FieldBuilder_CanSetNullValues()
        {
            var builder = Message.Build(Any.Message())[1][3];
            builder.RawValues = null;
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void FieldBuilder_CanSetNullFieldRepetitions()
        {
            var builder = Message.Build(Any.Message())[1][3];
            builder.SetFieldRepetitions(null);
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void FieldBuilder_SettingNullFieldRepetitionsAtIndexDoesNothing()
        {
            var builder = Message.Build(Any.Message())[1][3];
            builder.SetFieldRepetitions(null);
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void FieldBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetition(1, repetition1)
                .SetFieldRepetition(2, repetition2);
            builder.RawValue.Should().Be($"{repetition1}~{repetition2}");
        }

        [Test]
        public void FieldBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetition(2, repetition2)
                .SetFieldRepetition(1, repetition1);
            builder.RawValue.Should().Be($"{repetition1}~{repetition2}");
        }

        [Test]
        public void FieldBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetitions(3, repetition1, repetition2);
            builder.RawValue.Should().Be($"~~{repetition1}~{repetition2}");
        }

        [Test]
        public void FieldBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1][3];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(1, 1, component1)
                .SetComponent(1, 2, component2);
            builder.RawValue.Should().Be($"{component1}^{component2}");
        }

        [Test]
        public void FieldBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(1, 2, component2)
                .SetComponent(1, 1, component1);
            builder.RawValue.Should().Be($"{component1}^{component2}");
        }

        [Test]
        public void FieldBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponents(1, component1, component2);
            builder.RawValue.Should().Be($"{component1}^{component2}");
        }

        [Test]
        public void FieldBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, 1, 1, subcomponent1)
                .SetSubcomponent(1, 1, 2, subcomponent2);
            builder.RawValue.Should().Be($"{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void FieldBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, 1, 2, subcomponent2)
                .SetSubcomponent(1, 1, 1, subcomponent1);
            builder.RawValue.Should().Be($"{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void FieldBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponents(1, 1, 1, subcomponent1, subcomponent2);
            builder.RawValue.Should().Be($"{subcomponent1}&{subcomponent2}");
        }

        [Test]
        public void FieldBuilder_CanSetAllSubcomponents()
        {
            var builder = Message.Build()[1][3];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetSubcomponents(1, 1, 1, val0, val1, val2);
            builder.RawValue.Should().Be(string.Join("&", val0, val1, val2));
        }

        [Test]
        public void FieldBuilder_CanSetAllComponents()
        {
            var builder = Message.Build()[1][3];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetComponents(1, 1, val0, val1, val2);
            builder.RawValue.Should().Be(string.Join("^", val0, val1, val2));
        }

        [Test]
        public void FieldBuilder_CanSetAllRepetitions()
        {
            var builder = Message.Build()[1][3];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetFieldRepetitions(1, val0, val1, val2);
            builder.RawValue.Should().Be(string.Join("~", val0, val1, val2));
        }

        [Test]
        public void FieldBuilder_SettingNullRepetitionsDoesNothing()
        {
            var builder = Message.Build()[1][3];
            var value = builder.RawValue;
            builder.SetFieldRepetitions(null);
            builder.RawValue.Should().Be(value);
        }

        [Test]
        public void FieldBuilder_SettingNullRepetitionsAtIndexDoesNothing()
        {
            var builder = Message.Build()[1][3];
            var value = builder.RawValue;
            builder.SetFieldRepetitions(0, null);
            builder.RawValue.Should().Be(value);
        }

        [Test]
        public void FieldBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3];
            builder.Encoding.FieldDelimiter.Should().Be('|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            builder.Encoding.FieldDelimiter.Should().Be(':');
        }

        [Test]
        public void FieldBuilder_ChangesEncodingCharactersIfSelfChanges()
        {
            var builder = Message.Build()[1][2];
            builder.SetField("$~\\&");
            builder.Encoding.ComponentDelimiter.Should().Be('$');
        }

        [Test]
        public void FieldBuilder_HasDelimiterFromMessage()
        {
            var message = Message.Build();
            var builder = message[1][3];
            builder.Delimiter.Should().Be(message.Encoding.RepetitionDelimiter);
        }
    }
}