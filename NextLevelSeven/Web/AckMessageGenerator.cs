using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Web
{
    static public class AckMessageGenerator
    {
        static IMessage Generate(IMessage message, string code)
        {
            var sourceMsh = message["MSH"].First();
            var result = new Message(sourceMsh.Value);
            var targetMsh = result[1];
            var msa = result[2];

            targetMsh[5].Value = sourceMsh[3].Value;
            targetMsh[6].Value = sourceMsh[4].Value;
            targetMsh[3].Value = sourceMsh[5].Value;
            targetMsh[4].Value = sourceMsh[6].Value;

            msa[0].Value = "MSA";
            msa[1].Value = code;
            msa[2].Value = sourceMsh[10].Value;

            return result;
        }

        static public IMessage GenerateError(IMessage message)
        {
            return Generate(message, "AE");
        }

        static public IMessage GenerateReject(IMessage message)
        {
            return Generate(message, "AR");
        }
        
        static public IMessage GenerateSuccess(IMessage message)
        {
            return Generate(message, "AA");
        }
    }
}
