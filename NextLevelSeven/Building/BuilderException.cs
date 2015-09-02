using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Building
{
    [Serializable]
    public class BuilderException : Exception
    {
        public BuilderException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}