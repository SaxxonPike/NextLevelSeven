using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class ParserBaseUnitTests : ParsingTestFixture
    {
        [TestMethod]
        public void Parser_DeleteThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.DeleteDescendant(-1));
        }

        [TestMethod]
        public void Parser_MoveThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.MoveDescendant(2, -1));
            AssertAction.Throws<ElementException>(() => message.MoveDescendant(-1, 2));
        }

        [TestMethod]
        public void Parser_InsertThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            AssertAction.Throws<ElementException>(() => message.InsertDescendant(Mock.String(), -1)); 
        }

        [TestMethod]
        public void Parser_InsertElementThrowsIfInvalidIndex()
        {
            var message = Message.Parse(Mock.Message())[2][1];
            var element = message[1];
            AssertAction.Throws<ElementException>(() => message.InsertDescendant(element, -1));
        }

        [TestMethod]
        public void Parser_GetsNextIndex()
        {
            var message = Message.Parse(Mock.Message());
            Assert.AreEqual(message.ValueCount + 1, message.NextIndex);
        }

        [TestMethod]
        public void Parser_CanBeSorted()
        {
            var messages = new List<IElementParser>();
            var message0 = Message.Parse(ExampleMessages.Minimum);
            var message1 = Message.Parse(ExampleMessages.Minimum);
            message0[1][3].Value = "2";
            message0[1][4].Value = "1";
            messages.Add(message0);
            messages.Add(message1);
            messages.Sort();
            Assert.AreEqual(messages[0], message1);
            Assert.AreEqual(messages[1], message0);
        }

        [TestMethod]
        public void Parser_CanBeSortedGenerically()
        {
            var messages = new List<object>();
            var message0 = (IElement)Message.Parse(ExampleMessages.Minimum);
            var message1 = (IElement)Message.Parse(ExampleMessages.Minimum);
            message0[1][3].Value = "2";
            message0[1][4].Value = "1";
            messages.Add(message0);
            messages.Add(message1.Value);
            messages.Sort();
            Assert.AreEqual(messages[0], message1.Value);
            Assert.AreEqual(messages[1], message0);
        }

        [TestMethod]
        public void Parsers_HashCodeComesFromString()
        {
            IElementParser message = Message.Parse(ExampleMessages.Minimum);
            Assert.AreEqual(message.GetHashCode(), ExampleMessages.Minimum.GetHashCode());
        }

        [TestMethod]
        public void Parsers_AreEquivalentWhenValuesMatch()
        {
            IElementParser segment0 = Message.Parse(ExampleMessages.Standard)[1];
            IElementParser segment1 = Message.Parse(ExampleMessages.Standard)[1];
            Assert.AreNotSame(segment0, segment1);
            Assert.IsTrue(((IEquatable<IElement>)segment0).Equals(segment1));
        }

        [TestMethod]
        public void Parsers_AreEquivalentWhenReferencesMatch()
        {
            IElementParser segment = Message.Parse(ExampleMessages.Standard)[1];
            var generic = (object) segment;
            Assert.IsTrue(segment.Equals(generic));
        }

        [TestMethod]
        public void Parsers_AreEquivalentToStringsGenerically()
        {
            var segment0 = Message.Parse(ExampleMessages.Standard)[1];
            Assert.IsTrue(segment0.Equals(segment0.Value));
        }

        [TestMethod]
        public void Parsers_AreEquivalentToStrings()
        {
            var segment0 = Message.Parse(ExampleMessages.Standard)[1];
            Assert.IsTrue(((IEquatable<string>)segment0).Equals(segment0.Value));
        }

        [TestMethod]
        public void Parsers_AreNotEquivalentWhenNull()
        {
            IElementParser field = Message.Parse(ExampleMessages.Standard)[1][3];
            field.Value = null;
            Assert.IsFalse(field.Equals(null));
        }

    }
}
