using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Xml
{
    [TestClass]
    public class V2XmlTests : XmlTestFixture
    {
        [TestMethod]
        public void V2XmlConverter_CanConvertBuilderToXml()
        {
            var message = Message.Build(ExampleMessages.Standard);
            var document = message.ToXml();
            var documentContents = document.InnerXml;
            Assert.IsNotNull(documentContents);
        }

        [TestMethod]
        public void V2XmlConverter_CanConvertParserToXml()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var document = message.ToXml();
            var documentContents = document.InnerXml;
            Assert.IsNotNull(documentContents);
        }

        [TestMethod]
        public void V2XmlConverter_ConversionIsIdenticalBetweenMessages()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            var parser = Message.Parse(ExampleMessages.Standard);
            Assert.AreEqual(builder.ToXml().InnerXml, parser.ToXml().InnerXml);
        }

        [TestMethod]
        public void V2XmlConverter_CanConvertXmlToBuilder()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void V2XmlConverter_CanConvertXmlToParser()
        {
            Assert.Inconclusive();
        }
    }
}