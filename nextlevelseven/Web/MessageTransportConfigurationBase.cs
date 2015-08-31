using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Web
{
    /// <summary>
    /// A base class for background transport configurations. This is an abstract class.
    /// </summary>
    abstract public class MessageTransportConfigurationBase
    {
        /// <summary>
        /// Application name for use in MSH segments.
        /// </summary>
        public string OwnApplication;

        /// <summary>
        /// Facility name for use in MSH segments.
        /// </summary>
        public string OwnFacility;

        /// <summary>
        /// Port number to use for establishing connections.
        /// </summary>
        public int Port;
    }
}
