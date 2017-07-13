using System.Globalization;

namespace NextLevelSeven.Diagnostics
{
    /// <summary>Contains error message processing logic for multiple languages.</summary>
    public static partial class ErrorMessages
    {
        /// <summary>Language to fall back when the selected language does not have a translation.</summary>
        private static readonly ErrorMessageLanguage FallbackLanguage = new English();

        /// <summary>Backing store for Language.</summary>
        private static ErrorMessageLanguage _language;

        /// <summary>Currently selected language.</summary>
        private static ErrorMessageLanguage Language
        {
            get
            {
                if (_language == null)
                {
                    SetLanguage();
                }
                return _language;
            }
        }

        /// <summary>Get an error message for the currently selected language.</summary>
        /// <param name="code">Error code to get the message for.</param>
        /// <param name="extraInfo">Extra information to pass for string formatting.</param>
        /// <returns></returns>
        public static string Get(ErrorCode code, params object[] extraInfo)
        {
            var message = Language.GetMessage(code);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = FallbackLanguage.GetMessage(code);
            }
            return $"{string.Format(message ?? "Unknown error.", extraInfo)} (NL7-{(int) code})";
        }

        /// <summary>Set the selected language for error messages.</summary>
        /// <param name="language">Two-letter language code. If more than two characters are passed, only the first two are used.</param>
        public static void SetLanguage(string language = null)
        {
            _language = null;
            var cultureName = language ?? CultureInfo.CurrentCulture.Name.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(cultureName) || cultureName.Length < 2)
            {
                cultureName = "en";
            }

            switch (cultureName.Substring(0, 2))
            {
                case "en":
                    _language = new English();
                    break;
                case "de":
                    _language = new Deutsch();
                    break;
            }

            if (_language == null)
            {
                _language = FallbackLanguage;
            }
        }

        /// <summary>Base class for error message languages.</summary>
        private abstract class ErrorMessageLanguage
        {
            /// <summary>Get the error message for this language.</summary>
            /// <param name="code">Error code to get the message for.</param>
            /// <returns>Error string.</returns>
            public abstract string GetMessage(ErrorCode code);
        }
    }
}