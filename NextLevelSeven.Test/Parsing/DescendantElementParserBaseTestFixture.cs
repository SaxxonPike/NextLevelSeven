using System;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    public abstract class DescendantElementParserBaseTestFixture<TParser, TElement> : ElementParserBaseTestFixture<TParser, TElement>
        where TParser : IElementParser, TElement
        where TElement : IElement
    {
        [Test]
        public void Clone_HasNoMessage()
        {
            var parser = BuildParser().Clone();
            parser.Message.Should().BeNull();
        }

        [Test]
        public void Ancestor_Exists()
        {
            Ancestor_ExistsAsType(typeof(TParser));
        }

        [Test]
        public void Ancestor_ExistsAsBaseElement()
        {
            Ancestor_ExistsAsType(typeof(TElement));
        }

        [Test]
        [TestCase(typeof(IElement))]
        public void Ancestor_ExistsAsType(Type t)
        {
            InvokeGetter(t, "Ancestor", BuildParser())
                .Should().NotBeNull();
        }
    }
}
