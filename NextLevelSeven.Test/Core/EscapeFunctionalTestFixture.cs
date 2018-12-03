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
            var test = $"{leftString}{delimiter}{middleString}{delimiter}{rightString}";
            var expected = $"{leftString}{escapeCode}{middleString}{escapeCode}{rightString}";
            Test_Escape(expected, test);
        }

        private static void Test_UnEscape(string expected, string test)
        {
            var messageParser = Message.Parse();
            var messageBuilder = Message.Build();
            messageParser.Encoding.UnEscape(test).Should().Be(expected);
            messageBuilder.Encoding.UnEscape(test).Should().Be(expected);
        }

        private static void Test_SingleDelimiterUnEscape(string expectedCode, string sourceCode)
        {
            var leftString = Any.String();
            var middleString = Any.String();
            var rightString = Any.String();
            var test = $"{leftString}{sourceCode}{middleString}{sourceCode}{rightString}";
            var expected = $"{leftString}{expectedCode}{middleString}{expectedCode}{rightString}";
            Test_UnEscape(expected, test);
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
            var message = $"{Any.String()}\\H\\{Any.String()}";
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_NormalTextMarker()
        {
            var message = $"{Any.String()}\\N\\{Any.String()}";
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_MultiByteCharacterSetLongEscapeSequence()
        {
            var message = $"{Any.String()}\\MABCDEF\\{Any.String()}";
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_MultiByteCharacterSetShortEscapeSequence()
        {
            var message = $"{Any.String()}\\MABCD\\{Any.String()}";
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_LocallyDefinedEscapeSequence()
        {
            var message = $"{Any.String()}\\Z{Any.String()}\\{Any.String()}";
            Test_Escape(message, message);
        }

        [Test]
        public void Escape_DoesNotConvert_SingleByteCharacterSetEscapeSequence()
        {
            var message = $"{Any.String()}\\CABCD\\{Any.String()}";
            Test_Escape(message, message);
        }
        
        [Test]
        public void Escape_Converts_BinaryData()
        {
            Test_SingleDelimiterEscape("\x0D\x0A", "\\X0D\\\\X0A\\");
        }

        [Test]
        public void UnEscape_Converts_ComponentCharacters()
        {
            Test_SingleDelimiterUnEscape("^", "\\S\\");
        }

        [Test]
        public void UnEscape_Converts_EscapeCharacters()
        {
            Test_SingleDelimiterUnEscape("\\", "\\E\\");
        }

        [Test]
        public void UnEscape_Converts_FieldCharacters()
        {
            Test_SingleDelimiterUnEscape("|", "\\F\\");
        }

        [Test]
        public void UnEscape_Converts_PartialEscapeSequences()
        {
            // this contains what looks like the start of a variable length
            // locally defined sequence but is only partially existing.
            Test_UnEscape("\\Zork", "\\E\\Zork");
        }

        [Test]
        public void UnEscape_Converts_RepetitionCharacters()
        {
            Test_SingleDelimiterUnEscape("~", "\\R\\");
        }

        [Test]
        public void UnEscape_Converts_SubcomponentCharacters()
        {
            Test_SingleDelimiterUnEscape("&", "\\T\\");
        }

        [Test]
        public void UnEscape_Converts_BinaryData()
        {
            Test_SingleDelimiterUnEscape("\x0D\x0A", "\\X0D\\\\X0A\\");
        }

        [Test]
        public void UnEscape_DoesNotConvert_HighlightTextMarker()
        {
            var message = $"{Any.String()}\\H\\{Any.String()}";
            Test_UnEscape(message, message);
        }

        [Test]
        public void UnEscape_DoesNotConvert_NormalTextMarker()
        {
            var message = $"{Any.String()}\\N\\{Any.String()}";
            Test_UnEscape(message, message);
        }

        [Test]
        public void UnEscape_DoesNotConvert_MultiByteCharacterSetLongEscapeSequence()
        {
            var message = $"{Any.String()}\\MABCDEF\\{Any.String()}";
            Test_UnEscape(message, message);
        }

        [Test]
        public void UnEscape_DoesNotConvert_MultiByteCharacterSetShortEscapeSequence()
        {
            var message = $"{Any.String()}\\MABCD\\{Any.String()}";
            Test_UnEscape(message, message);
        }

        [Test]
        public void UnEscape_DoesNotConvert_LocallyDefinedEscapeSequence()
        {
            var message = $"{Any.String()}\\Z{Any.String()}\\{Any.String()}";
            Test_UnEscape(message, message);
        }

        [Test]
        public void UnEscape_DoesNotConvert_SingleByteCharacterSetEscapeSequence()
        {
            var message = $"{Any.String()}\\CABCD\\{Any.String()}";
            Test_UnEscape(message, message);
        }
    }
}