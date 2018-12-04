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
    public class SubcomponentParserFunctionalTestFixture : DescendantElementParserBaseTestFixture<ISubcomponentParser, ISubcomponent>
    {
        protected override ISubcomponentParser BuildParser()
        {
            return Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
        }

        [Test]
        public void Subcomponent_CanGetKey()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1];
            element.Key.Should().Be("MSH1.3.1.1.1");
        }

        [Test]
        public void Subcomponent_UsesSameEncodingAsMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1];
            element.Encoding.Should().Be(message.Encoding);
        }

        [Test]
        public void Subcomponent_CanGetMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1];
            element.Message.Should().Be(message);
        }

        [Test]
        public void Subcomponent_CloneHasNoMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1].Clone();
            element.Message.Should().BeNull();
        }

        [Test]
        public void Subcomponent_HasNoDescendants()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            element.Descendants.Should().BeEmpty();
        }

        [Test]
        public void Subcomponent_HasComponentAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            element.Ancestor.Should().NotBeNull();
        }

        [Test]
        public void Subcomponent_HasGenericComponentAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1] as ISubcomponent;
            element.Ancestor.Should().NotBeNull();
        }

        [Test]
        public void Subcomponent_HasGenericAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1] as IElementParser;
            (element.Ancestor as IComponent).Should().NotBeNull();
        }

        [Test]
        public void Subcomponent_HasOneValue()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            var val0 = Any.String();
            element.RawValue = val0;
            Assert.AreEqual(1, element.ValueCount);
            Assert.AreEqual(element.RawValue, val0);
            Assert.AreEqual(1, element.RawValues.Count());
        }

        [Test]
        public void Subcomponent_ThrowsWhenMovingElements()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            element.RawValue = Any.String();
            var newMessage = element.Clone();
            newMessage.Invoking(m => m[2].Move(3)).Should().Throw<ElementException>();
        }

        [Test]
        public void Subcomponent_Throws_WhenIndexed()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            element.Invoking(e => e[1].RawValue.Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void Subcomponent_CanAddDescendantsAtEnd()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[2][3][4][1];
            var count = subcomponent.ValueCount;
            var id = Any.String();
            subcomponent[count + 1].RawValue = id;
            Assert.AreEqual(count + 1, subcomponent.ValueCount,
                @"Number of elements after appending at the end of a subcomponent is incorrect.");
        }

        [Test]
        public void Subcomponent_CanWriteStringValue()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var value = Any.String();
            subcomponent.RawValue = value;
            Assert.AreEqual(value, subcomponent.RawValue, "Value mismatch after write.");
        }

        [Test]
        public void Subcomponent_CanWriteNullValue()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var value = Any.String();
            subcomponent.RawValue = value;
            subcomponent.RawValue = null;
            Assert.IsNull(subcomponent.RawValue, "Value mismatch after write.");
        }
    }
}