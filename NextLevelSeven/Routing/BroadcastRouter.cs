using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// A router that broadcasts all incoming messages to a list of routers. The route is successful when any of these routes is successful.
    /// </summary>
    sealed public class BroadcastRouter : IRouter
    {
        /// <summary>
        /// Create a route finder with the specified routes initially in the list.
        /// </summary>
        /// <param name="routers">Routers to put in the list.</param>
        public BroadcastRouter(params IRouter[] routers)
        {
            foreach (var router in routers)
            {
                Routers.Add(router);
            }
        }

        /// <summary>
        /// Create a route finder with the specified routes initially in the list.
        /// </summary>
        /// <param name="routers">Routers to put in the list.</param>
        public BroadcastRouter(IEnumerable<IRouter> routers)
        {
            foreach (var router in routers)
            {
                Routers.Add(router);
            }            
        }

        /// <summary>
        /// List of routers to broadcast messages to.
        /// </summary>
        public readonly IList<IRouter> Routers = new List<IRouter>();

        /// <summary>
        /// Route a message to all other routers in the list.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>True, if any of the routes are successful.</returns>
        public bool Route(IMessage message)
        {
            var success = false;
            foreach (var router in Routers)
            {
                success |= router.Route(message);
            }
            return success;
        }
    }
}
