using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    static internal class Escaping
    {
        /// <summary>
        /// Escape a string so it can be represented in raw HL7 form.
        /// </summary>
        /// <param name="config">Encoding character configuration.</param>
        /// <param name="data">Data to escape.</param>
        /// <returns>Escaped string.</returns>
        static public string Escape(EncodingConfiguration config, string data)
        {
            return data;
        }

        /// <summary>
        /// Unscape a string from raw HL7 form.
        /// </summary>
        /// <param name="config">Encoding character configuration.</param>
        /// <param name="data">Data to remove escaping from.</param>
        /// <returns>Unescaped string.</returns>
        static public string UnEscape(EncodingConfiguration config, string data)
        {
            return data;
        }
    }
}
