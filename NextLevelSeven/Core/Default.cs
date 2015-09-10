using System;
using System.Diagnostics;

namespace NextLevelSeven.Core
{
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