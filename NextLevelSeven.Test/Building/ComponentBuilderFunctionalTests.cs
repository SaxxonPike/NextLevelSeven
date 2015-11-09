using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class ComponentBuilderFunctionalTests : BuildingTestFixture
    {
        [Test]
        public void ComponentBuilder_CanMoveDescendants()
        {
            var builder = Message.Build(ExampleMessages.Variety)[1][3][1][1];
            var val1 = MockFactory.String();
            var val2 = MockFactory.String();
            builder[1].Value = val1;
            builder[2].Value = val2;
            builder.Move(1, 2);
            builder[1].Value.Should().Be(val2);
            builder[2].Value.Should().Be(val1);
        }

        [Test]
        public void ComponentBuilder_ExistsWithValue()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1];
            builder.Value = MockFactory.String();
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void ComponentBuilder_DoesNotExistWithNullValue()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1];
            builder.Value = null;
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void ComponentBuilder_DoesNotExistAfterErasing()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1];
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
            var builder = Message.Build(ExampleMessages.Variety)[1][3][1][1];
            builder.Subcomponents.Count().Should().Be(2);
        }

        [Test]
        public void ComponentBuilder_ClearsSubcomponentsWithNoParameters()
        {
            var builder = Message.Build()[1][3][1][1];
            builder.Values = new[] {MockFactory.String(), MockFactory.String()};
            builder.SetSubcomponents();
            builder.Value.Should().BeNull();
        }

        [Test]
        public void ComponentBuilder_DoesNotClearSubcomponentsWithNoParametersAndIndex()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            builder.Values = new[] { val0, val1 };
            builder.SetSubcomponents(1);
            builder.Value.Should().Be(string.Format("{0}&{1}", val0, val1));
        }

        [Test]
        public void ComponentBuilder_CanSetComponent()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = MockFactory.String();
            builder.SetComponent(val0);
            builder.Value.Should().Be(val0);
        }

        [Test]
        public void ComponentBuilder_CanSetSubcomponent()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = MockFactory.String();
            builder.SetSubcomponent(2, val0);
            builder.Value.Should().Be(string.Format("&{0}", val0));
        }

        [Test]
        public void ComponentBuilder_CanSetSubcomponents()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            var val2 = MockFactory.String();
            builder.SetSubcomponents(val0, val1, val2);
            builder.Value.Should().Be(string.Format("{0}&{1}&{2}", val0, val1, val2));
        }

        [Test]
        public void ComponentBuilder_CanSetSubcomponentsAtIndex()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            var val2 = MockFactory.String();
            builder.SetSubcomponents(2, val0, val1, val2);
            builder.Value.Should().Be(string.Join("&", "", val0, val1, val2));
        }

        [Test]
        public void ComponentBuilder_CanSetValues()
        {
            var builder = Message.Build()[1][3][1][1];
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            var val2 = MockFactory.String();
            builder.Values = new[] {val0, val1, val2};
            builder.Value.Should().Be(string.Format("{0}&{1}&{2}", val0, val1, val2));
        }

        [Test]
        public void ComponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1];
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void ComponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1] as IComponent;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void ComponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1] as IElement;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void ComponentBuilder_CanGetSubcomponent()
        {
            var builder = Message.Build(ExampleMessages.Variety);
            builder[1][3][2][2][2].Value.Should().NotBeNull();
            builder[1][3][2][2][2].Value.Should().Be(builder.Segment(1).Field(3).Repetition(2).Component(2).Subcomponent(2).Value);
        }

        [Test]
        public void ComponentBuilder_CanGetValue()
        {
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{2}^{0}&{1}", val0, val1, MockFactory.String()))[1][3][1][2];
            builder.Value.Should().Be(string.Format("{0}&{1}", val0, val1));
        }

        [Test]
        public void ComponentBuilder_CanGetValues()
        {
            var val1 = MockFactory.String();
            var val2 = MockFactory.String();
            var val3 = MockFactory.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                MockFactory.String(), val1, val2, val3))[1][3][2][2];
            builder.Values.Should().Equal(val2, val3);
        }

        [Test]
        public void ComponentBuilder_CanBeCloned()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][2][2];
            var clone = builder.Clone();
            builder.Should().NotBeSameAs(clone);
            builder.ToString().Should().Be(clone.ToString());
        }

        [Test]
        public void ComponentBuilder_CanBeClonedGenerically()
        {
            IElement builder = Message.Build(MockFactory.Message())[1][3][2][2];
            var clone = builder.Clone();
            builder.Should().NotBeSameAs(clone);
            builder.ToString().Should().Be(clone.ToString());
        }

        [Test]
        public void ComponentBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = MockFactory.String();
            var subcomponent2 = MockFactory.String();

            builder
                .SetSubcomponent(1, subcomponent1)
                .SetSubcomponent(2, subcomponent2);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void ComponentBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = MockFactory.String();
            var subcomponent2 = MockFactory.String();

            builder
                .SetSubcomponent(2, subcomponent2)
                .SetSubcomponent(1, subcomponent1);
            builder.Value.Should().Be(string.Format("{0}&{1}", subcomponent1, subcomponent2));
        }

        [Test]
        public void ComponentBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build()[1][3][1][1];
            var subcomponent1 = MockFactory.String();
            var subcomponent2 = MockFactory.String();

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