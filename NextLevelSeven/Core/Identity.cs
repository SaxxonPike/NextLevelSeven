using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Contains identity information for a sender or receiver.
    /// </summary>
    sealed public class Identity : IIdentity
    {
        /// <summary>
        /// Create a new Identity with the defaults.
        /// </summary>
        public Identity()
        {
            Application = ApplicationDefault;
            Facility = FacilityDefault;
        }

        /// <summary>
        /// Create a new Identity with the specified values.
        /// </summary>
        /// <param name="application">Application name.</param>
        /// <param name="facility">Facility name/</param>
        public Identity(string application, string facility)
        {
            Application = application;
            Facility = facility;
        }

        /// <summary>
        /// Get or set the application name.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Get the default application name, which is the name of the currently executing process.
        /// </summary>
        static public string ApplicationDefault { get { return Process.GetCurrentProcess().ProcessName; } }

        /// <summary>
        /// Get or set the facility name.
        /// </summary>
        public string Facility { get; set; }

        /// <summary>
        /// Get the default facility name, which is the name of the current user's account domain name. If not present, the user's account name is used.
        /// </summary>
        static public string FacilityDefault { get { return Environment.UserDomainName; } }
    }
}
