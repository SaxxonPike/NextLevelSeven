using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class RepetitionBuilderFunctionalTestFixture : DescendantElementBuilderBaseTestFixture<IRepetitionBuilder, IRepetition>
    {
        protected override IRepetitionBuilder BuildBuilder()
        {
            return Message.Build(ExampleMessageRepository.Standard)[1][3][1];
        }

        [Test]
        public void RepetitionBuilder_CanSetAllSubcomponents()
        {
            var builder = Message.Build()[1][3][1];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetSubcomponents(1, 1, val0, val1, val2);
            builder.Value.Should().Be(string.Join("&", val0, val1, val2));
        }

        [Test]
        public void RepetitionBuilder_CanSetAllComponents()
        {
            var builder = Message.Build()[1][3][1];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetComponents(1, val0, val1, val2);
            builder.Value.Should().Be(string.Join("^", val0, val1, val2));
        }

        [Test]
        public void RepetitionBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1];
            builder.Ancestor[1].Should().BeSameAs(builder);
        }

        [Test]
        public void RepetitionBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1] as IRepetition;
            builder.Ancestor[1].Should().BeSameAs(builder);
        }

        [Test]
        public void RepetitionBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1] as IElement;
            builder.Ancestor[1].Should().BeSameAs(builder);
        }

        [Test]
        public void RepetitionBuilder_CanGetComponent_ThroughField()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1][3][1][2].Value.Should().Be(builder.Segment(1).Field(3).Component(2).Value)
                .And.NotBeNull();
        }

        [Test]
        public void RepetitionBuilder_CanGetComponent()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1][3][2][2].Value.Should().Be(builder.Segment(1).Field(3).Repetition(2).Component(2).Value)
                .And.NotBeNull();
        }

        [Test]
        public void RepetitionBuilder_CanGetValue()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{2}~{0}^{1}", val0, val1, Any.String()))[1][3][2];
            builder.Value.Should().Be(string.Format("{0}^{1}", val0, val1));
        }

        [Test]
        public void RepetitionBuilder_CanGetValues()
        {
            var val1 = Any.String();
            var val2 = Any.String();
            var val3 = Any.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Any.String(), val1, val2, val3))[1][3][2];
            builder.Values.Should().Equal(val1, string.Format("{0}&{1}", val2, val3));
        }

        [Test]
        public void RepetitionBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(1, component1)
                .SetComponent(2, component2);
            builder.Value.Should().Be(string.Format("{0}^{1}", component1, component2));
        }

        [Test]
        public void RepetitionBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponent(2, component2)
                .SetComponent(1, component1);
            builder.Value.Should().Be(string.Format("{0}^{1}", component1, component2));
        }

        [Test]
        public void RepetitionBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1];
            var component1 = Any.String();
            var component2 = Any.String();

            builder
                .SetComponents(3, component1, component2);
            builder.Value.Should().Be(string.Format("^^{0}^{1}", component1, component2));
        }

        [Test]
        public void RepetitionBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3][1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, 1, subcomponent1)
                .SetSubcomponent(1, 2, subcomponent2);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void RepetitionBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, 2, subcomponent2)
                .SetSubcomponent(1, 1, subcomponent1);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void RepetitionBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponents(3, 1, subcomponent1, subcomponent2);
            builder.Value.Should().Be(string.Format("^^{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void RepetitionBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3][1];
            builder.Encoding.FieldDelimiter.Should().Be('|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            builder.Encoding.FieldDelimiter.Should().Be(':');
        }
    }
}