using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class BuilderParserEqualityFunctionalTestFixture : CoreBaseTestFixture
    {
        private ParserComparer _comparer;

        [SetUp]
        public void Initialize()
        {
            _comparer = new ParserComparer(Message.Build(), Message.Parse());
        }

        [Test]
        public void Parsers_ParseIdentical_OnStandardMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.Standard);
        }

        [Test]
        public void Parsers_ParseIdentical_OnA04Message()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.A04);
        }

        [Test]
        public void Parsers_ParseIdentical_OnBadSubComponentMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.BadSubComponent);
        }

        [Test]
        public void Parsers_ParseIdentical_OnMinimumMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.Minimum);
        }

        [Test]
        public void Parsers_ParseIdentical_OnMultipleObrMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.MultipleObr);
        }

        [Test]
        public void Parsers_ParseIdentical_OnMultiplePidMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.MultiplePid);
        }

        [Test]
        public void Parsers_ParseIdentical_OnRepeatingNameMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.RepeatingName);
        }

        [Test]
        public void Parsers_ParseIdentical_OnVersionlessMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessageRepository.VersionlessMessage);
        }
    }
}