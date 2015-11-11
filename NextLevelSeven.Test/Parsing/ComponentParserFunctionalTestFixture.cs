using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class ComponentParserFunctionalTestFixture : DescendantElementParserBaseTestFixture<IComponentParser, IComponent>
    {
        protected override IComponentParser BuildParser()
        {
            return Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1];
        }

        [Test]
        public void Component_CanMoveSubcomponents()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1];
            element.Values = new[] {Any.String(), Any.String(), Any.String(), Any.String()};
            var newMessage = element.Clone();
            newMessage[2].Move(3);
            newMessage[3].Value.Should().Be(element[2].Value);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Component_Throws_WhenIndexedBelowOne()
        {
            var component = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1];
            component[0].Value.Should().BeNull();
        }

        [Test]
        public void Component_CanAddDescendantsAtEnd()
        {
            var component = Message.Parse(ExampleMessageRepository.Standard)[2][3][4][1];
            var count = component.ValueCount;
            var id = Any.String();
            component[count + 1].Value = id;
            component.ValueCount.Should().Be(count + 1);
        }

        [Test]
        public void Component_CanGetSubcomponents()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var id3 = Any.String();
            var id4 = Any.String();
            var component =
                Message.Parse(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}", id1, id2, id3, id4))[1][3][2][2];
            component.Subcomponents.Count().Should().Be(2);
        }

        [Test]
        public void Component_CanGetSubcomponentsGenerically()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var id3 = Any.String();
            var id4 = Any.String();
            IComponent component =
                Message.Parse(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}", id1, id2, id3, id4))[1][3][2][2];
            component.Subcomponents.Count().Should().Be(2);
        }

        [Test]
        public void Component_CanGetSubcomponentsByIndexer()
        {
            var id1 = Any.String();
            var id2 = Any.String();
            var id3 = Any.String();
            var id4 = Any.String();
            var component =
                Message.Parse(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}", id1, id2, id3, id4))[1][3][2][2][2];
            component.Value.Should().Be(id4);
        }

        [Test]
        public void Component_CanDeleteSubcomponent()
        {
            var message = Message.Parse("MSH|^~\\&|\rTST|123^456&ABC~789^012");
            var component = message[2][1][1][2];
            ElementExtensions.Delete(component, 1);
            message.Value.Should().Be("MSH|^~\\&|\rTST|123^ABC~789^012");
        }

        [Test]
        public void Component_CanWriteStringValue()
        {
            var component = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1];
            var value = Any.String();
            component.Value = value;
            component.Value.Should().Be(value);
        }

        [Test]
        public void Component_CanWriteNullValue()
        {
            var component = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1];
            var value = Any.String();
            component.Value = value;
            component.Value = null;
            component.Value.Should().BeNull();
        }
    }
}