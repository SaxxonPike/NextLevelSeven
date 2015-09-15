using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Native;
using NextLevelSeven.Native.Elements;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Extensions to the IMessageBuilder interface.
    /// </summary>
    static public class MessageBuilderExtensions
    {
        /// <summary>
        ///     Copy the contents of this builder to an HL7 message.
        /// </summary>
        /// <param name="builder">Builder to get data from.</param>
        /// <returns>Converted message.</returns>
        static public INativeMessage ToNativeMessage(this IMessageBuilder builder)
        {
            return new NativeMessage(builder.Value);
        }
    }
}
