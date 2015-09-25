using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Specification
{
    /// <summary>Represents an error thrown when specification validation fails.</summary>
    [Serializable]
    public class ValidationException : Exception
    {
        // ReSharper disable once CoVariantArrayConversion

        /// <summary>Create a specification validation exception.</summary>
        internal ValidationException(ErrorCode code, params string[] extraInfo)
            : base(ErrorMessages.Get(code, extraInfo))
        {
        }
    }
}