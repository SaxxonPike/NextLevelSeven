using System;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class EscapeFunctionalTestFixture : CoreBaseTestFixture
    {
        private static void Test_Escape(string expected, string test)
        {
            var messageParser = Message.Parse();
            var messageBuilder = Message.Build();
            messageParser.Encoding.Escape(test).Should().Be(expected);
            messageBuilder.Encoding.Escape(test).Should().Be(expected);
        }

        private static void Test_SingleDelimiterEscape(string delimiter, string escapeCode)
        {
            var leftString = Any.String();
            var middleString = Any.String();
            var rightString = Any.String();
            var test = String.Format("{0}{3}{1}{3}{2}", leftString, middleString, rightString, delimiter);
            var expected = string.Format("{0}{3}{1}{3}{2}", leftString, middleString, rightString, escapeCode);
            Test_Escape(expected, test);
        }

        [Test]
        public void Escape_Converts_Null()
        {
            Test_Escape(null, null);
        }

        [Test]
        public void Escape_Converts_ComponentCharacters()
        {
            Test_SingleDelimiterEscape("^", "\\S\\");
        }

        [Test]
        public void Escape_Converts_EscapeCharacters()
        {
            Test_SingleDelimiterEscape("\\", "\\E\\");
        }

        [Test]
        public void Escape_Converts_FieldCharacters()
        {
            Test_SingleDelimiterEscape("|", "\\F\\");
        }

        [Test]
        public void Escape_Converts_PartialEscapeSequences()
        {
            // this contains what looks like the start of a variable length
            // locally defined sequence but is only partially existing.
            Test_Escape("\\E\\Zork", "\\Zork");
        }

        [Test]
        public void Escape_Converts_RepetitionCharacters()
        {
            Test_SingleDelimiterEscape("~", "\\R\\");
        }

        [Test]
        public void Escape_Converts_SubcomponentCharacters()
        {
            Test_SingleDelimiterEscape("&", "\\T\\");
        }

        [Test]
        public void Escape_DoesNotConvert_HighlightTextMarker()
        {
            var message = String.Format("{0}\\H\\{1}", Any.String(), Any.String());
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_NormalTextMarker()
        {
            var message = String.Format("{0}\\N\\{1}", Any.String(), Any.String());
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_MultiByteCharacterSetLongEscapeSequence()
        {
            var message = String.Format("{0}\\MABCDEF\\{1}", Any.String(), Any.String());
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_MultiByteCharacterSetShortEscapeSequence()
        {
            var message = String.Format("{0}\\MABCD\\{1}", Any.String(), Any.String());
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_LocallyDefinedEscapeSequence()
        {
            var message = String.Format("{0}\\Z{2}\\{1}", Any.String(), Any.String(), Any.String());
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_SingleByteCharacterSetEscapeSequence()
        {
            var message = String.Format("{0}\\CABCD\\{1}", Any.String(), Any.String());
            Test_Escape(message, message);
        }
    }
}