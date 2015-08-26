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
    /// <summary>
    /// A threaded HTTP sender queue for HL7v2.
    /// </summary>
    public class BackgroundMessageSender : BackgroundTransportBase, IDisposable
    {
        /// <summary>
        /// Create a sender, which will monitor the queue and perform POSTs to the target address with messages in it.
        /// </summary>
        /// <param name="address"></param>
        public BackgroundMessageSender(string address)
        {
            var config = new MessageSenderConfiguration();
            config.Address = address;

            Thread = new Thread(BackgroundMessageThreadMain);
            Thread.Start(config);
        }

        /// <summary>
        /// Main method for the sender. This runs on a separate thread.
        /// </summary>
        /// <param name="configObject">Sender configuration.</param>
        void BackgroundMessageThreadMain(object configObject)
        {
            var config = (MessageSenderConfiguration)configObject;

            try
            {
                while (!Disposed && !Aborted)
                {
                    Ready = true;
                    Thread.Sleep(500);
                    while (Count > 0)
                    {
                        Ready = false;
                        var message = Messages.Dequeue();
                        var request = WebRequest.Create(config.Address);
                        request.Method = "POST";
                        request.ContentType = "x-application/hl7-v2+er7";

                        using (var requestStream = request.GetRequestStream())
                        using (var messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message.Contents.ToString())))
                        {
                            messageStream.CopyTo(requestStream);
                        }

                        var response = request.GetResponse();
                        using (var responseStream = response.GetResponseStream())
                        using (var messageStream = new MemoryStream())
                        {
                            if (responseStream != null)
                            {
                                responseStream.CopyTo(messageStream);
                                messageStream.Position = 0;

                                var messageReader = new HL7StreamReader(messageStream);
                                var responseMessage = messageReader.Read();
                                var responseMsa = responseMessage["MSA"].FirstOrDefault();
                                if (responseMsa != null)
                                {
                                    if (responseMsa[1].Value != "AA")
                                    {
                                        Retry(message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Aborted = true;
            }
        }

        /// <summary>
        /// Stop sending and clean up.
        /// </summary>
        public void Dispose()
        {
            if (Thread != null)
            {
                Ready = false;
                Disposed = true;
                Thread = null;
            }
        }

        /// <summary>
        /// If true, Dispose() has been called on this sender.
        /// </summary>
        public bool Disposed
        {
            get;
            private set;
        }
    }
}
