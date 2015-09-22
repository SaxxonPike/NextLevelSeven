using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ParserComparisonTests : CoreTestFixture
    {
        private ParserComparer _comparer;

        [TestInitialize]
        public void Initialize()
        {
            _comparer = new ParserComparer(Message.Build(), Message.Parse());
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnStandardMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.Standard);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnA04Message()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.A04);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnBadSubComponentMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.BadSubComponent);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnMinimumMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.Minimum);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnMultipleObrMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.MultipleObr);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnMultiplePidMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.MultiplePid);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnRepeatingNameMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.RepeatingName);
        }

        [TestMethod]
        public void Parsers_ParseIdentical_OnVersionlessMessage()
        {
            _comparer.AssertParseEquivalent(ExampleMessages.VersionlessMessage);
        }
    }
}