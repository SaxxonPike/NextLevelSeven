using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Web
{
    public class BackgroundMessageReceiver : IDisposable
    {
        public BackgroundMessageReceiver(int port)
        {
            var config = new MessageReceiverConfiguration();

            Thread = new Thread(BackgroundMessageThreadMain);
            Thread.Start(config);
        }

        void BackgroundMessageThreadMain(object configObject)
        {
            var config = (MessageReceiverConfiguration)configObject;
            var listener = new HttpListener();

            try
            {
                listener.Prefixes.Add("http://*:" + config.Port + "/");
                listener.Start();

                while (true)
                {
                    var context = listener.GetContext();
                    var httpRequest = context.Request;
                    var httpResponse = context.Response;
                    IMessage request;

                    using (var mem = new MemoryStream())
                    {
                        httpRequest.InputStream.CopyTo(mem);
                        mem.Position = 0;
                        var reader = new HL7StreamReader(mem);
                        request = reader.Read();
                    }

                    var response = AckMessageGenerator.GenerateSuccess(request);
                    httpResponse.ContentType = "x-application/hl7-v2+er7";

                    using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(response.ToString())))
                    {
                        mem.CopyTo(httpResponse.OutputStream);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                listener.Stop();
            }
        }

        Thread Thread
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (Thread != null)
            {
                Thread.Abort();
                Thread = null;
            }
        }
    }
}
