using System;
using System.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Manages default settings. TODO: convert this into local settings.
    /// </summary>
    internal static class Default
    {
        public static string Application
        {
            get { return Process.GetCurrentProcess().ProcessName; }
        }

        public static string Facility
        {
            get { return Environment.UserDomainName; }
        }
    }
}