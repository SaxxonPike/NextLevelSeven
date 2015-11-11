using FluentAssertions;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Diagnostics
{
    [TestFixture]
    public class ErrorMessageUnitTestFixture : DiagnosticsBaseTestFixture
    {
        [SetUp]
        public void ErrorMessages_Initializer()
        {
            ErrorMessages.SetLanguage("en");
        }

        [TearDown]
        public void ErrorMessages_UnInitializer()
        {
            ErrorMessages.SetLanguage();
        }

        [Test]
        public void ErrorMessages_ReturnsMessagesWithErrorCodes()
        {
            var message = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            message.Should().EndWith("(NL7-1)");
        }

        [Test]
        public void ErrorMessages_FallsBack_WhenUntranslatedError()
        {
            var message = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            ErrorMessages.SetLanguage("de");
            var germanMessage = ErrorMessages.Get(ErrorCode.DoNotTranslateThisMessageForTestingPurposes);
            message.Should().Be(germanMessage);
            message.Should().NotBe(ErrorMessages.Get(ErrorCode.Unspecified));
        }

        [Test]
        public void ErrorMessages_SetsLanguageToDefault_WhenPassingGarbage()
        {
            ErrorMessages.SetLanguage("de");
            var message = ErrorMessages.Get(ErrorCode.Unspecified);
            ErrorMessages.SetLanguage(Any.String());
            message.Should().NotBe(ErrorMessages.Get(ErrorCode.Unspecified));
        }

        [Test]
        public void ErrorMessages_SetsLanguageToDefault_WhenPassingEmpty()
        {
            ErrorMessages.SetLanguage("de");
            var message = ErrorMessages.Get(ErrorCode.Unspecified);
            ErrorMessages.SetLanguage(string.Empty);
            message.Should().NotBe(ErrorMessages.Get(ErrorCode.Unspecified));
        }

        [Test]
        public void ErrorMessages_SetsLanguageToDefault_WhenPassingNull()
        {
            ErrorMessages.SetLanguage("de");
            var message = ErrorMessages.Get(ErrorCode.Unspecified);
            ErrorMessages.SetLanguage();
            message.Should().NotBe(ErrorMessages.Get(ErrorCode.Unspecified));
        }

        [Test]
        public void ErrorMessages_InvalidCodeReturnsGenericMessage()
        {
            var message = ErrorMessages.Get((ErrorCode) int.MaxValue);
            message.Should().Contain("Unknown error");
            message.Should().Contain(string.Format("(NL7-{0})", int.MaxValue));
        }
    }
}