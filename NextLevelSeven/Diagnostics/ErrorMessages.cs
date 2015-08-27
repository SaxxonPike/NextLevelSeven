using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Diagnostics
{
    /// <summary>
    /// Contains error message processing logic for multiple languages.
    /// </summary>
    static public partial class ErrorMessages
    {
        abstract private class ErrorMessageLanguage
        {
            public abstract string GetMessage(ErrorCode code);
        }

        private static readonly ErrorMessageLanguage FallbackLanguage = new English();

        private static ErrorMessageLanguage _language;
        static private ErrorMessageLanguage Language
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

        static public string Get(ErrorCode code)
        {
            var message = Language.GetMessage(code);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = FallbackLanguage.GetMessage(code);
            }

            return string.Format("{0} (NL7-{1})", message ?? "Unknown error.", (int)code);
        }

        static public void SetLanguage(string language = null)
        {
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
    }
}
