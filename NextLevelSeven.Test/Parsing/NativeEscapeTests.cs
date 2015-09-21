using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class NativeEscapeTests : NativeTestFixture
    {
        private static void Test_Escape(string expected, string test)
        {
            var message = Message.Parse();
            Assert.AreEqual(expected, message.Escape(test));
        }

        private static void Test_SingleDelimiterEscape(string delimiter, string escapeCode)
        {
            var leftString = Randomized.String();
            var middleString = Randomized.String();
            var rightString = Randomized.String();
            var test = String.Format("{0}{3}{1}{3}{2}", leftString, middleString, rightString, delimiter);
            var expected = string.Format("{0}{3}{1}{3}{2}", leftString, middleString, rightString, escapeCode);
            Test_Escape(expected, test);
        }

        [TestMethod]
        public void Escape_Converts_ComponentCharacters()
        {
            Test_SingleDelimiterEscape("^", "\\S\\");
        }

        [TestMethod]
        public void Escape_Converts_EscapeCharacters()
        {
            Test_SingleDelimiterEscape("\\", "\\E\\");
        }

        [TestMethod]
        public void Escape_Converts_FieldCharacters()
        {
            Test_SingleDelimiterEscape("|", "\\F\\");
        }

        [TestMethod]
        public void Escape_Converts_PartialEscapeSequences()
        {
            // this contains what looks like the start of a variable length
            // locally defined sequence but is only partially existing.
            Test_Escape("\\E\\Zork", "\\Zork");
        }

        [TestMethod]
        public void Escape_Converts_RepetitionCharacters()
        {
            Test_SingleDelimiterEscape("~", "\\R\\");
        }

        [TestMethod]
        public void Escape_Converts_SubcomponentCharacters()
        {
            Test_SingleDelimiterEscape("&", "\\T\\");
        }

        [TestMethod]
        public void Escape_DoesNotConvert_HighlightTextMarker()
        {
            var message = String.Format("{0}\\H\\{1}", Randomized.String(), Randomized.String());
            Test_Escape(message, message);
        }

        [TestMethod]
        public void Escape_DoesNotConvert_NormalTextMarker()
        {
            var message = String.Format("{0}\\N\\{1}", Randomized.String(), Randomized.String());
            Test_Escape(message, message);
        }

        [TestMethod]
        public void Escape_DoesNotConvert_MultiByteCharacterSetLongEscapeSequence()
        {
            var message = String.Format("{0}\\MABCDEF\\{1}", Randomized.String(), Randomized.String());
            Test_Escape(message, message);
        }

        [TestMethod]
        public void Escape_DoesNotConvert_MultiByteCharacterSetShortEscapeSequence()
        {
            var message = String.Format("{0}\\MABCD\\{1}", Randomized.String(), Randomized.String());
            Test_Escape(message, message);
        }

        [TestMethod]
        public void Escape_DoesNotConvert_LocallyDefinedEscapeSequence()
        {
            var message = String.Format("{0}\\Z{2}\\{1}", Randomized.String(), Randomized.String(), Randomized.String());
            Test_Escape(message, message);
        }

        [TestMethod]
        public void Escape_DoesNotConvert_SingleByteCharacterSetEscapeSequence()
        {
            var message = String.Format("{0}\\CABCD\\{1}", Randomized.String(), Randomized.String());
            Test_Escape(message, message);
        }
    }
}