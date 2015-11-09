using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Diagnostics
{
    [TestFixture]
    public class ErrorMessageUnitTests : DiagnosticsTestFixture
    {
        [TestInitialize]
        public void ErrorMessages_Initializer()
        {
            ErrorMessages.SetLanguage("en");
        }

        [TestCleanup]
        public void ErrorMessages_UnInitializer()
        {
            ErrorMessages.SetLanguage();
        }

        [Test]
        public void ErrorMessages_ReturnsMessagesWithErrorCodes()
        {
            var message = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            Assert.IsTrue(message.EndsWith("(NL7-1)"), @"Error code is not properly returned in the error string.");
        }

        [Test]
        public void ErrorMessages_FallsBack_WhenUntranslatedError()
        {
            var message = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            ErrorMessages.SetLanguage("de");
            var germanMessage = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            Assert.AreEqual(message, germanMessage, @"German did not fall back to English.");
            Assert.AreNotEqual(message, ErrorMessages.Get(ErrorCode.Unspecified),
                @"German and English returned the same error string.");
        }

        [Test]
        public void ErrorMessages_SetsLanguageToDefault_WhenPassingGarbage()
        {
            ErrorMessages.SetLanguage("de");
            var message = ErrorMessages.Get(ErrorCode.Unspecified);
            ErrorMessages.SetLanguage(MockFactory.String());
            Assert.AreNotEqual(message, ErrorMessages.Get(ErrorCode.Unspecified),
                @"German and English returned the same error string.");
        }

        [Test]
        public void ErrorMessages_SetsLanguageToDefault_WhenPassingEmpty()
        {
            ErrorMessages.SetLanguage("de");
            var message = ErrorMessages.Get(ErrorCode.Unspecified);
            ErrorMessages.SetLanguage(string.Empty);
            Assert.AreNotEqual(message, ErrorMessages.Get(ErrorCode.Unspecified),
                @"German and English returned the same error string.");
        }

        [Test]
        public void ErrorMessages_SetsLanguageToDefault_WhenPassingNull()
        {
            ErrorMessages.SetLanguage("de");
            var message = ErrorMessages.Get(ErrorCode.Unspecified);
            ErrorMessages.SetLanguage();
            Assert.AreNotEqual(message, ErrorMessages.Get(ErrorCode.Unspecified),
                @"German and English returned the same error string.");
        }

        [Test]
        public void ErrorMessages_InvalidCodeReturnsGenericMessage()
        {
            var message = ErrorMessages.Get((ErrorCode) int.MaxValue);
            Assert.IsTrue(message.Contains("Unknown error"));
            Assert.IsTrue(message.Contains(string.Format("(NL7-{0})", int.MaxValue)));
        }
    }
}