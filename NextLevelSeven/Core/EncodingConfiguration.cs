using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    sealed internal class EncodingConfiguration
    {
        public EncodingConfiguration(IElement messageElement)
        {
            Message = messageElement;
        }

        public char ComponentDelimiter
        {
            get { return Message.Value[4]; }
        }

        public char EscapeDelimiter
        {
            get { return Message.Value[6]; }
        }

        IElement Message
        {
            get;
            set;
        }

        public char RepetitionDelimiter
        {
            get { return Message.Value[5]; }
        }

        public char SubcomponentDelimiter
        {
            get { return Message.Value[7]; }
        }
    }
}
