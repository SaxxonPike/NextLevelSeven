using System.Linq;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class ComponentBuilderFunctionalTestFixture : DescendantElementBuilderBaseTestFixture<IComponentBuilder, IComponent>
    {
        protected override IComponentBuilder BuildBuilder()
        {
            return Message.Build(ExampleMessageRepository.Standard)[1][3][1][1];
        }

        [Test]
        public void ComponentBuilder_CanMoveDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety)[1][3][1][1];
            var val1 = Any.String();
            var val2 = Any.String();
            builder[1].Value = val1;
            builder[2].Value = val2;
            builder.Move(1, 2);
            builder[1].Value.Should().Be(val2);
            builder[2].Value.Should().Be(val1);
        }

        [Test]
        public void ComponentBuilder_ExistsWithValue()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1];
            builder.Value = Any.String();
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void ComponentBuilder_DoesNotExistWithNullValue()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1];
            builder.Value = null;
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void ComponentBuilder_DoesNotExistAfterErasing()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1];
            builder.Erase();
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void ComponentBuilder_HasDelimiter()
        {
            var builder = Message.Build()[1][3][1][1];
            builder.Delimiter.Should().Be('&');
        }

        [Test]
        public void ComponentBuilder_HasSubcomponents()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety)[1][3][1][1];
            builder.Subcomponents.Count().Should().Be(2);
        }

        [Test]
        public void ComponentBuilder_ClearsSubcomponentsWithNoParameters()
        {
            var builder = Message.Build()[1][3][1][1];
            builder.Values = new[] {Any.String(), Any.String()};
            builder.SetSubcomponents();
            builder.Value.Should().BeNull();
        }

        [Test]
        public void ComponentBuilder_DoesNotClearSubcomponentsWithNoParametersAndIndex()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Any.String();
            var val1 = Any.String();
            builder.Values = new[] { val0, val1 };
            builder.SetSubcomponents(1);
            builder.Value.Should().Be(string.Format("{0}&{1}", val0, val1));
        }

        [Test]
        public void ComponentBuilder_CanSetComponent()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Any.String();
            builder.SetComponent(val0);
            builder.Value.Should().Be(val0);
        }

        [Test]
        public void ComponentBuilder_CanSetSubcomponent()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Any.String();
            builder.SetSubcomponent(2, val0);
            builder.Value.Should().Be(string.Format("&{0}", val0));
        }

        [Test]
        public void ComponentBuilder_CanSetSubcomponents()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetSubcomponents(val0, val1, val2);
            builder.Value.Should().Be(string.Format("{0}&{1}&{2}", val0, val1, val2));
        }

        [Test]
        public void ComponentBuilder_CanSetSubcomponentsAtIndex()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.SetSubcomponents(2, val0, val1, val2);
            builder.Value.Should().Be(string.Join("&", "", val0, val1, val2));
        }

        [Test]
        public void ComponentBuilder_CanSetValues()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = Any.String();
            var val1 = Any.String();
            var val2 = Any.String();
            builder.Values = new[] {val0, val1, val2};
            builder.Value.Should().Be(string.Format("{0}&{1}&{2}", val0, val1, val2));
        }

        [Test]
        public void ComponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1];
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void ComponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1] as IComponent;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void ComponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1] as IElement;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void ComponentBuilder_CanGetSubcomponent()
        {
            var builder = Message.Build(ExampleMessageRepository.Variety);
            builder[1][3][2][2][2].Value.Should().NotBeNull();
            builder[1][3][2][2][2].Value.Should().Be(builder.Segment(1).Field(3).Repetition(2).Component(2).Subcomponent(2).Value);
        }

        [Test]
        public void ComponentBuilder_CanGetValue()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{2}^{0}&{1}", val0, val1, Any.String()))[1][3][1][2];
            builder.Value.Should().Be(string.Format("{0}&{1}", val0, val1));
        }

        [Test]
        public void ComponentBuilder_CanGetValues()
        {
            var val1 = Any.String();
            var val2 = Any.String();
            var val3 = Any.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Any.String(), val1, val2, val3))[1][3][2][2];
            builder.Values.Should().Equal(val2, val3);
        }

        [Test]
        public void ComponentBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(1, subcomponent1)
                .SetSubcomponent(2, subcomponent2);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void ComponentBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponent(2, subcomponent2)
                .SetSubcomponent(1, subcomponent1);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void ComponentBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = Any.String();
            var subcomponent2 = Any.String();

            builder
                .SetSubcomponents(3, subcomponent1, subcomponent2);
            builder.Value.Should().Be(string.Format("&&{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void ComponentBuilder_ChangesEncodingCharactersIfMessageChanges()
        {
            var messageBuilder = Message.Build();
            var builder = messageBuilder[1][3][1][1];
            builder.Encoding.FieldDelimiter.Should().Be('|');
            messageBuilder.Encoding.FieldDelimiter = ':';
            builder.Encoding.FieldDelimiter.Should().Be(':');
        }
    }
}