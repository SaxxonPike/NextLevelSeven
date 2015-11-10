using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public class ComponentParserFunctionalTests : ParsingTestFixture
    {
        [Test]
        public void Component_HasRepetitionAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1];
            Assert.IsNotNull(element.Ancestor);
        }

        [Test]
        public void Component_HasGenericRepetitionAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1] as IComponent;
            Assert.IsNotNull(element.Ancestor);
        }

        [Test]
        public void Component_HasGenericAncestor()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1] as IElementParser;
            Assert.IsNotNull(element.Ancestor as IRepetition);
        }

        [Test]
        public void Component_CanMoveSubcomponents()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3][1][1];
            element.Values = new[] {Any.String(), Any.String(), Any.String(), Any.String()};
            var newMessage = element.Clone();
            newMessage[2].Move(3);
            Assert.AreEqual(element[2].Value, newMessage[3].Value);
        }

        [Test]
        public void Component_Throws_WhenIndexedBelowOne()
        {
            var component = Message.Parse(ExampleMessages.Standard)[1][3][1][1];
            string value = null;
            AssertAction.Throws<ElementException>(() => { value = component[0].Value; });
            Assert.IsNull(value);
        }

        [Test]
        public void Component_CanBeCloned()
        {
            var component = Message.Parse(ExampleMessages.Standard)[1][3][1][1];
            var clone = component.Clone();
            Assert.AreNotSame(component, clone, "Cloned component is the same referenced object.");
            Assert.AreEqual(component.Value, clone.Value, "Cloned component has different contents.");
        }

        [Test]
        public void Component_CanBeClonedGenerically()
        {
            IElement component = Message.Parse(ExampleMessages.Standard)[1][3][1][1];
            var clone = component.Clone();
            Assert.AreNotSame(component, clone, "Cloned component is the same referenced object.");
            Assert.AreEqual(component.Value, clone.Value, "Cloned component has different contents.");
        }

        [Test]
        public void Component_CanAddDescendantsAtEnd()
        {
            var component = Message.Parse(ExampleMessages.Standard)[2][3][4][1];
            var count = component.ValueCount;
            var id = Any.String();
            component[count + 1].Value = id;
            Assert.AreEqual(count + 1, component.ValueCount,
                @"Number of elements after appending at the end of a component is incorrect.");
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
            Assert.AreEqual(2, component.Subcomponents.Count());
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
            Assert.AreEqual(2, component.Subcomponents.Count());
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
            Assert.AreEqual(id4, component.Value);
        }

        [Test]
        public void Component_CanDeleteSubcomponent()
        {
            var message = Message.Parse("MSH|^~\\&|\rTST|123^456&ABC~789^012");
            var component = message[2][1][1][2];
            ElementExtensions.Delete(component, 1);
            Assert.AreEqual("MSH|^~\\&|\rTST|123^ABC~789^012", message.Value, @"Message was modified unexpectedly.");
        }

        [Test]
        public void Component_CanWriteStringValue()
        {
            var component = Message.Parse(ExampleMessages.Standard)[1][3][1][1];
            var value = Any.String();
            component.Value = value;
            Assert.AreEqual(value, component.Value, "Value mismatch after write.");
        }

        [Test]
        public void Component_CanWriteNullValue()
        {
            var component = Message.Parse(ExampleMessages.Standard)[1][3][1][1];
            var value = Any.String();
            component.Value = value;
            component.Value = null;
            Assert.IsNull(component.Value, "Value mismatch after write.");
        }
    }
}