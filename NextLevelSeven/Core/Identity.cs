using System;
using System.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Contains identity information for a sender or receiver.
    /// </summary>
    public sealed class Identity : IIdentity
    {
        /// <summary>
        ///     Create a new Identity with the defaults.
        /// </summary>
        public Identity()
        {
            Application = ApplicationDefault;
            Facility = FacilityDefault;
        }

        /// <summary>
        ///     Create a new Identity with the specified values.
        /// </summary>
        /// <param name="application">Application name.</param>
        /// <param name="facility">Facility name/</param>
        public Identity(string application, string facility)
        {
            Application = application;
            Facility = facility;
        }

        /// <summary>
        ///     Get the default application name, which is the name of the currently executing process.
        /// </summary>
        public static string ApplicationDefault
        {
            get { return Process.GetCurrentProcess().ProcessName; }
        }

        /// <summary>
        ///     Get the default facility name, which is the name of the current user's account domain name. If not present, the
        ///     user's account name is used.
        /// </summary>
        public static string FacilityDefault
        {
            get { return Environment.UserDomainName; }
        }

        /// <summary>
        ///     Get or set the application name.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        ///     Get or set the facility name.
        /// </summary>
        public string Facility { get; set; }
    }
}