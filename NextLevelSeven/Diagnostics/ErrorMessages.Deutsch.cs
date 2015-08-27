using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Diagnostics
{
    static public partial class ErrorMessages
    {
        /// <summary>
        /// German language translation.
        /// </summary>
        class Deutsch : ErrorMessageLanguage
        {
            public override string GetMessage(ErrorCode code)
            {
                switch (code)
                {
                    case ErrorCode.Unspecified:
                        return "Unbekannter Fehler.";
                }
                return null;
            }
        }
    }
}
