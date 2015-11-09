using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class BuilderParserEqualityFunctionalTests : CoreTestFixture
    {
        private ParserComparer _comparer;

        [TestInitialize]
        public void Initialize()
        {
            _comparer = new ParserComparer(Message.Build(), Message.Parse());
        }

        [Test]
        public void Parsers_ParseIdentical_OnStandardMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.Standard);
        }

        [Test]
        public void Parsers_ParseIdentical_OnA04Message()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.A04);
        }

        [Test]
        public void Parsers_ParseIdentical_OnBadSubComponentMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.BadSubComponent);
        }

        [Test]
        public void Parsers_ParseIdentical_OnMinimumMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.Minimum);
        }

        [Test]
        public void Parsers_ParseIdentical_OnMultipleObrMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.MultipleObr);
        }

        [Test]
        public void Parsers_ParseIdentical_OnMultiplePidMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.MultiplePid);
        }

        [Test]
        public void Parsers_ParseIdentical_OnRepeatingNameMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.RepeatingName);
        }

        [Test]
        public void Parsers_ParseIdentical_OnVersionlessMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.VersionlessMessage);
        }
    }
}