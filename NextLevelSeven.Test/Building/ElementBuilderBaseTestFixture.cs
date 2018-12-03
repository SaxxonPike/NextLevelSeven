using System;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public abstract class ElementBuilderBaseTestFixture<TBuilder, TElement> : BuildingBaseTestFixture
        where TBuilder : IElementBuilder, TElement
        where TElement : IElement
    {
        protected abstract TBuilder BuildBuilder();

        [Test]
        public void Clone_SucceedsAsBaseElement()
        {
            Clone_SucceedsAsType(typeof(TElement));
        }

        [Test]
        [TestCase(typeof(IElement))]
        public void Clone_SucceedsAsType(Type type)
        {
            CloneAndTest(type, BuildBuilder());
        }
    }
}
