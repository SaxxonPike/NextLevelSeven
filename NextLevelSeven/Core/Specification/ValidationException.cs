using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core.Specification
{
    /// <summary>
    ///     Represents an error thrown when specification validation fails.
    /// </summary>
    [Serializable]
    public class ValidationException : Exception
    {
        /// <summary>
        ///     Create a specification validation exception.
        /// </summary>
        internal ValidationException(ErrorCode code, params string[] extraInfo)
            : base(ErrorMessages.Get(code, extraInfo))
        {
        }
    }
}
