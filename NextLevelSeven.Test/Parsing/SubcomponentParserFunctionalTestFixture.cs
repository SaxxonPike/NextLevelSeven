using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class SubcomponentParserFunctionalTestFixture : ParsingBaseTestFixture
    {
        [Test]
        public void Subcomponent_CanGetKey()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1];
            Assert.AreEqual("MSH1.3.1.1.1", element.Key);
        }

        [Test]
        public void Subcomponent_UsesSameEncodingAsMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1];
            Assert.AreSame(message.Encoding, element.Encoding);
        }

        [Test]
        public void Subcomponent_CanGetMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1];
            Assert.AreSame(message, element.Message);
        }

        [Test]
        public void Subcomponent_CloneHasNoMessage()
        {
            var message = Message.Parse(ExampleMessageRepository.Minimum);
            var element = message[1][3][1][1][1].Clone();
            Assert.IsNull(element.Message);
        }

        [Test]
        public void Subcomponent_HasNoDescendants()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            Assert.AreEqual(0, element.Descendants.Count());
        }

        [Test]
        public void Subcomponent_HasComponentAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            Assert.IsNotNull(element.Ancestor);
        }

        [Test]
        public void Subcomponent_HasGenericComponentAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1] as ISubcomponent;
            Assert.IsNotNull(element.Ancestor);
        }

        [Test]
        public void Subcomponent_HasGenericAncestor()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1] as IElementParser;
            Assert.IsNotNull(element.Ancestor as IComponent);
        }

        [Test]
        public void Subcomponent_HasOneValue()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            var val0 = Any.String();
            element.Value = val0;
            Assert.AreEqual(1, element.ValueCount);
            Assert.AreEqual(element.Value, val0);
            Assert.AreEqual(1, element.Values.Count());
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Subcomponent_ThrowsWhenMovingElements()
        {
            var element = Message.Parse(ExampleMessageRepository.Minimum)[1][3][1][1][1];
            element.Value = Any.String();
            var newMessage = element.Clone();
            newMessage[2].Move(3);
        }

        [Test]
        [ExpectedException(typeof(ElementException))]
        public void Subcomponent_Throws_WhenIndexed()
        {
            var element = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            element[1].Value.Should().BeNull();
        }

        [Test]
        public void Subcomponent_CanBeCloned()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var clone = subcomponent.Clone();
            Assert.AreNotSame(subcomponent, clone, "Cloned subcomponent is the same referenced object.");
            Assert.AreEqual(subcomponent.Value, clone.Value, "Cloned subcomponent has different contents.");
        }

        [Test]
        public void Subcomponent_CanBeClonedGenerically()
        {
            var subcomponent = (IElement)Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var clone = subcomponent.Clone();
            Assert.AreNotSame(subcomponent, clone, "Cloned subcomponent is the same referenced object.");
            Assert.AreEqual(subcomponent.Value, clone.Value, "Cloned subcomponent has different contents.");
        }

        [Test]
        public void Subcomponent_CanAddDescendantsAtEnd()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[2][3][4][1];
            var count = subcomponent.ValueCount;
            var id = Any.String();
            subcomponent[count + 1].Value = id;
            Assert.AreEqual(count + 1, subcomponent.ValueCount,
                @"Number of elements after appending at the end of a subcomponent is incorrect.");
        }

        [Test]
        public void Subcomponent_CanWriteStringValue()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var value = Any.String();
            subcomponent.Value = value;
            Assert.AreEqual(value, subcomponent.Value, "Value mismatch after write.");
        }

        [Test]
        public void Subcomponent_CanWriteNullValue()
        {
            var subcomponent = Message.Parse(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var value = Any.String();
            subcomponent.Value = value;
            subcomponent.Value = null;
            Assert.IsNull(subcomponent.Value, "Value mismatch after write.");
        }
    }
}