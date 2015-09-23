using System.Globalization;
using System.Linq;
using System.Xml;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Xml
{
    /// <summary>
    ///     Conversion methods for HL7v2 and XML.
    /// </summary>
    internal static class V2Xml
    {
        /// <summary>
        ///     Convert an XML document into a message builder.
        /// </summary>
        /// <param name="document">Document to convert.</param>
        /// <returns>Converted document.</returns>
        public static IMessageBuilder ConvertToBuilder(XmlDocument document)
        {
            var message = Message.Build();
            ImportXml(document, message);
            return message;
        }

        /// <summary>
        ///     Convert an XML document into a message parser.
        /// </summary>
        /// <param name="document">Document to convert.</param>
        /// <returns>Converted document.</returns>
        public static IMessageParser ConvertToParser(XmlDocument document)
        {
            var message = Message.Parse();
            ImportXml(document, message);
            return message;
        }

        /// <summary>
        ///     Convert a message into XML.
        /// </summary>
        /// <param name="message">Message to convert.</param>
        /// <returns>Converted message.</returns>
        public static XmlDocument ConvertToXml(IMessage message)
        {
            var document = new XmlDocument();
            ExportXml(message, document);
            return document;
        }

        /// <summary>
        ///     Sanitize string before XML output.
        /// </summary>
        /// <param name="value">Value to sanitize.</param>
        /// <returns>Sanitized string.</returns>
        private static string EscapeString(string value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        ///     Write message content into an XML document.
        /// </summary>
        /// <param name="message">Message to get content from.</param>
        /// <param name="document">Document to write into.</param>
        private static void ExportXml(IMessage message, XmlDocument document)
        {
            var details = message.Details;
            if (details.Type == null || details.Type.Length != 3)
            {
                throw new V2XmlException(ErrorCode.MessageTypeIsInvalid);
            }
            if (details.TriggerEvent != null && details.TriggerEvent.Length != 3)
            {
                throw new V2XmlException(ErrorCode.MessageTriggerEventIsInvalid);
            }

            var messageTag = (details.TriggerEvent == null)
                ? EscapeString(details.Type)
                : string.Concat(EscapeString(details.Type), "_", EscapeString(details.TriggerEvent));

            var messageElement = document.CreateElement(EscapeString(messageTag), "urn:hl7-org:v2xml");
            document.AppendChild(messageElement);
            ExportElement(document, messageElement, "", message);
        }

        /// <summary>
        ///     Write one element into an XML document.
        /// </summary>
        /// <param name="document">Document to write into.</param>
        /// <param name="ancestor">Ancestor XML element to write into.</param>
        /// <param name="nameSpace">Namespace used to build tag names.</param>
        /// <param name="element">Element to write.</param>
        private static void ExportElement(XmlDocument document, XmlNode ancestor, string nameSpace, IElement element)
        {
            // prepare descendant content
            var descendants = element.GetDescendantContent().ToList();

            // repetitions are encoded differently
            if (element is IField && descendants.Any())
            {
                foreach (var descendant in descendants)
                {
                    ExportElement(document, ancestor, nameSpace, descendant);
                }
                return;
            }

            // determine tag name
            string type;
            if (element is ISegment)
            {
                type = (element as ISegment).Type;
            }
            else if (element is IMessage)
            {
                type = string.Empty;
            }
            else
            {
                var index = (element is IRepetition) ? element.Ancestor.Index : element.Index;
                type = string.Concat(nameSpace, ".", index.ToString(CultureInfo.InvariantCulture));
            }

            // simplification of elements
            if (!element.HasSignificantDescendants())
            {
                var value = element.Value;
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                if (value == "\"\"")
                {
                    value = string.Empty;
                }

                var text = document.CreateElement(type);
                text.InnerText = EscapeString(value);
                ancestor.AppendChild(text);
                return;
            }

            // determine container
            XmlNode container;
            if (element is IMessage)
            {
                container = ancestor;
            }
            else
            {
                container = document.CreateElement(type);
            }

            // generate descendants
            foreach (var descendant in descendants)
            {
                ExportElement(document, container, type, descendant);
            }

            if (container != ancestor)
            {
                ancestor.AppendChild(container);
            }
        }

        /// <summary>
        ///     Read an XML document into a message.
        /// </summary>
        /// <param name="message">Message to write into.</param>
        /// <param name="document">Source document.</param>
        private static void ImportXml(XmlDocument document, IMessage message)
        {
        }
    }
}