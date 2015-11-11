using System.Linq;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
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
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Type_CannotMoveDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][0];
            builder.Move(1, 2);
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
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Type_ThrowsOnIndex()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][0];
            builder[1].Value.Should().BeNull();
        }

        [Test]
        public void FieldBuilder_Type_SetsRepetitionOne()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[2][0];
            var value = Any.String();
            builder.SetFieldRepetition(1, value);
            builder.Value.Should().Be(value);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Type_ThrowsOnSettingRepetitionsOtherThanOne([Values(0, 2)] int index)
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[2][0];
            var value = Any.String();
            builder.SetFieldRepetition(index, value);
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
            builder.Value = "$%^&";
            builder.Values.Should().BeEquivalentTo("$", "%", "^", "&");
        }

        [Test]
        public void FieldBuilder_Encoding_SetsValue()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Value = "$%^&";
            builder.Value.Should().Be("$%^&");
        }

        [Test]
        public void FieldBuilder_Encoding_SetsValues()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Values = new[] {"$", "#", "~", "@"};
            builder.Value.Should().Be("$#~@");
        }

        [Test]
        public void FieldBuilder_Encoding_CanNullify()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.Nullify();
            builder.Value.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Encoding_ThrowsOnSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder[1].Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Encoding_ThrowsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.SetFieldRepetition(0, "~@#$").Should().BeNull();
        }

        [Test]
        public void FieldBuilder_Encoding_ContainsCorrectDelimitersWithOversizedValue()
        {
            var message = Message.Build(ExampleMessageRepository.Standard);
            var builder = message[1][2];
            const string value = "!@#$%";
            builder.Value = value;
            message.Encoding.ComponentDelimiter.Should().Be('!');
            message.Encoding.RepetitionDelimiter.Should().Be('@');
            message.Encoding.EscapeCharacter.Should().Be('#');
            message.Encoding.SubcomponentDelimiter.Should().Be('$');
            builder.Value.Should().Be(value);
        }

        [Test]
        public void FieldBuilder_Encoding_SetsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][2];
            builder.SetFieldRepetition(1, "~@#$");
            builder.Value.Should().Be("~@#$");
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
            builder.Value.Should().Be("|");
        }

        [Test]
        public void FieldBuilder_Delimiter_GetsValues()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Values.Count().Should().Be(1);
        }

        [Test]
        public void FieldBuilder_Delimiter_SetsSingleValueFromOnlyFirstCharacter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Value = "$%^&";
            builder.Value.Should().Be("$");
        }

        [Test]
        public void FieldBuilder_Delimiter_SetsValuesFromOnlyFirstCharacter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Values = new[] {"$", "#"};
            builder.Value.Should().Be("$");
        }

        [Test]
        public void FieldBuilder_Delimiter_CanNullify()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.Nullify();
            builder.Value.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Delimiter_ThrowsOnSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder[1].Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void FieldBuilder_Delimiter_ThrowsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.SetFieldRepetition(0, "$").Should().BeNull();
        }

        [Test]
        public void FieldBuilder_Delimiter_SetsOnIndirectSubdivision()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][1];
            builder.SetFieldRepetition(1, "$");
            builder.Value.Should().Be("$");
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
            builder[1][3][2].Value.Should().NotBeNull();
            builder[1][3][2].Value.Should().Be(builder.Segment(1).Field(3).Repetition(2).Value);
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
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", val0, val1))[1][3];
            builder.Value.Should().Be(string.Format("{0}~{1}", val0, val1));
        }

        [Test]
        public void FieldBuilder_CanGetValues()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            var val3 = Any.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                val0, val1, val2, val3))[1][3];
            builder.Values.Should().Equal(val0, string.Format("{0}^{1}&{2}", val1, val2, val3));
        }

        [Test]
        public void FieldBuilder_CanSetNullValues()
        {
            var builder = Message.Build(Any.Message())[1][3];
            builder.Values = null;
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
            builder.Value.Should().Be(string.Format("{0}~{1}", repetition1, repetition2));
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
            builder.Value.Should().Be(string.Format("{0}~{1}", repetition1, repetition2));
        }

        [Test]
        public void FieldBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var repetition1 = Any.String();
            var repetition2 = Any.String();

            builder
                .SetFieldRepetitions(3, repetition1, repetition2);
            builder.Value.Should().Be(string.Format("~~{0}~{1}", repetition1, repetition2));
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
            builder.Value.Should().Be(string.Format("{0}^{1}", component1, component2));
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
            builder.Value.Should().Be(string.Format("{0}^{1}", component1, component2));
        }

        [Test]
        public void FieldBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponents(1, component1, component2);
            builder.Value.Should().Be(string.Format("{0}^{1}", component1, component2));
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
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
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
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void FieldBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponents(1, 1, 1, subcomponent1, subcomponent2);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void FieldBuilder_CanSetAllSubcomponents()
        {
            var builder = Message.Build()[1][3];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetSubcomponents(1, 1, 1, val0, val1, val2);
            builder.Value.Should().Be(string.Join("&", val0, val1, val2));
        }

        [Test]
        public void FieldBuilder_CanSetAllComponents()
        {
            var builder = Message.Build()[1][3];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetComponents(1, 1, val0, val1, val2);
            builder.Value.Should().Be(string.Join("^", val0, val1, val2));
        }

        [Test]
        public void FieldBuilder_CanSetAllRepetitions()
        {
            var builder = Message.Build()[1][3];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetFieldRepetitions(1, val0, val1, val2);
            builder.Value.Should().Be(string.Join("~", val0, val1, val2));
        }

        [Test]
        public void FieldBuilder_SettingNullRepetitionsDoesNothing()
        {
            var builder = Message.Build()[1][3];
            var value = builder.Value;
            builder.SetFieldRepetitions(null);
            builder.Value.Should().Be(value);
        }

        [Test]
        public void FieldBuilder_SettingNullRepetitionsAtIndexDoesNothing()
        {
            var builder = Message.Build()[1][3];
            var value = builder.Value;
            builder.SetFieldRepetitions(0, null);
            builder.Value.Should().Be(value);
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