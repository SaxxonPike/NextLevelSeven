using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Specification;

namespace NextLevelSeven.Test.Core.Specification
{
    [TestClass]
    public class CodedElementTests : SpecificationTestFixture
    {
        private string _alternateCodingSystemName;
        private string _alternateIdentifier;
        private string _alternateText;
        private string _codingSystemName;
        private ICodedElement _element;
        private string _identifier;
        private string _message;
        private string _text;

        [TestInitialize]
        public void Initialize()
        {
            _identifier = Randomized.String();
            _text = Randomized.String();
            _codingSystemName = Randomized.String();
            _alternateIdentifier = Randomized.String();
            _alternateText = Randomized.String();
            _alternateCodingSystemName = Randomized.String();
            _message = string.Format("MSH|^~\\&|{0}^{1}^{2}^{3}^{4}^{5}|{6}",
                _identifier, _text, _codingSystemName, _alternateIdentifier,
                _alternateText, _alternateCodingSystemName,
                Randomized.String());
            _element = Message.Create(_message)[1][3][1].AsCodedElement();
        }

        [TestMethod]
        public void CodedElement_Validate_Succeeds()
        {
            _element.Validate();
        }

        [TestMethod]
        public void CodedElement_Parses_Identifier()
        {
            Assert.AreEqual(_identifier, _element.Identifier);
        }

        [TestMethod]
        public void CodedElement_Parses_Text()
        {
            Assert.AreEqual(_text, _element.Text);
        }

        [TestMethod]
        public void CodedElement_Parses_CodingSystemName()
        {
            Assert.AreEqual(_codingSystemName, _element.CodingSystemName);
        }

        [TestMethod]
        public void CodedElement_Parses_AlternateIdentifier()
        {
            Assert.AreEqual(_alternateIdentifier, _element.AlternateIdentifier);
        }

        [TestMethod]
        public void CodedElement_Parses_AlternateText()
        {
            Assert.AreEqual(_alternateText, _element.AlternateText);
        }

        [TestMethod]
        public void CodedElement_Parses_AlternateCodingSystemName()
        {
            Assert.AreEqual(_alternateCodingSystemName, _element.AlternateCodingSystemName);
        }

        [TestMethod]
        public void CodedElement_Sets_Identifier()
        {
            var value = Randomized.String();
            _element.Identifier = value;
            Assert.AreEqual(value, _element.Identifier);
        }

        [TestMethod]
        public void CodedElement_Sets_Text()
        {
            var value = Randomized.String();
            _element.Text = value;
            Assert.AreEqual(value, _element.Text);
        }

        [TestMethod]
        public void CodedElement_Sets_CodingSystemName()
        {
            var value = Randomized.String();
            _element.CodingSystemName = value;
            Assert.AreEqual(value, _element.CodingSystemName);
        }

        [TestMethod]
        public void CodedElement_Sets_AlternateIdentifier()
        {
            var value = Randomized.String();
            _element.AlternateIdentifier = value;
            Assert.AreEqual(value, _element.AlternateIdentifier);
        }

        [TestMethod]
        public void CodedElement_Sets_AlternateText()
        {
            var value = Randomized.String();
            _element.AlternateText = value;
            Assert.AreEqual(value, _element.AlternateText);
        }

        [TestMethod]
        public void CodedElement_Sets_AlternateCodingSystemName()
        {
            var value = Randomized.String();
            _element.AlternateCodingSystemName = value;
            Assert.AreEqual(value, _element.AlternateCodingSystemName);
        }
    }
}