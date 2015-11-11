using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    public abstract class DescendantElementBuilderBaseTestFixture<TBuilder, TElement> : ElementBuilderBaseTestFixture<TBuilder, TElement>
        where TBuilder : IElementBuilder, TElement
        where TElement : IElement
    {
        [Test]
        public void Clone_HasNoMessage()
        {
            var parser = BuildBuilder().Clone();
            parser.Message.Should().BeNull();
        }

        [Test]
        public void Ancestor_Exists()
        {
            Ancestor_ExistsAsType(typeof(TBuilder));
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
            InvokeGetter(t, "Ancestor", BuildBuilder())
                .Should().NotBeNull();
        }
    }
}
