using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// A router that goes through a sequence of routes until one is found.
    /// </summary>
    sealed public class RouteFinder : IRouter
    {
        /// <summary>
        /// Create a route finder with the specified routes initially in the list.
        /// </summary>
        /// <param name="routers">Routers to put in the list.</param>
        public RouteFinder(params IRouter[] routers)
        {
            foreach (var router in routers)
            {
                Routes.Add(router);
            }
        }

        /// <summary>
        /// Create a route finder with the specified routes initially in the list.
        /// </summary>
        /// <param name="routers">Routers to put in the list.</param>
        public RouteFinder(IEnumerable<IRouter> routers)
        {
            foreach (var router in routers)
            {
                Routes.Add(router);
            }            
        }

        /// <summary>
        /// List of routes to go through.
        /// </summary>
        public readonly IList<IRouter> Routes = new List<IRouter>();

        /// <summary>
        /// Go through the list of routes, returning true when one is successful. If no routes succeed, returns false.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>True if route succeeds. False otherwise.</returns>
        public bool Route(IMessage message)
        {
            return Routes.Any(r => r.Route(message));
        }
    }
}
