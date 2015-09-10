using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Specification
{
    static internal class Default
    {
        public static string Application { get { return Process.GetCurrentProcess().ProcessName; } }
        public static string Facility { get { return Environment.UserDomainName; } }
    }
}
