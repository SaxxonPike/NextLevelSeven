using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Test.Diagnostics
{
    [TestClass]
    public class ErrorMessageTests
    {
        [TestMethod]
        public void ErrorMessages_ReturnsMessagesWithErrorCode()
        {
            var message = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            Assert.IsTrue(message.EndsWith("(NL7-20)"), @"Error code is not properly returned in the error string.");
        }

        [TestMethod]
        public void ErrorMessages_FallsBackOnUntranslatedError()
        {
            var message = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            ErrorMessages.SetLanguage("de");
            var germanMessage = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            Assert.AreEqual(message, germanMessage, @"German did not fall back to English.");
            Assert.AreNotEqual(message, ErrorMessages.Get(ErrorCode.Unspecified), @"German and English returned the same error string.");
        }
    }
}
