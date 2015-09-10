using System;
using System.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Manages default settings. TODO: convert this into local settings.
    /// </summary>
    internal static class Default
    {
        /// <summary>
        ///     Default "HD" Application when sending messages.
        /// </summary>
        public static readonly string Application = Process.GetCurrentProcess().ProcessName;

        /// <summary>
        ///     Default "HD" Facility when sending messages.
        /// </summary>
        public static readonly string Facility = Environment.UserDomainName;
    }
}