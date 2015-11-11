using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing
{
    [TestFixture]
    public abstract class ElementParserBaseTestFixture<TParser, TElement> : ParsingBaseTestFixture
        where TParser : IElementParser, TElement
        where TElement : IElement
    {
        protected abstract TParser BuildParser();

        [Test]
        public void Clone_SucceedsAsBaseElement()
        {
            Clone_SucceedsAsType(typeof(TElement));
        }

        [Test]
        [TestCase(typeof(IElement))]
        public void Clone_SucceedsAsType(Type type)
        {
            CloneAndTest(type, BuildParser());
        }
    }
}
