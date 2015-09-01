using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     A threaded HTTP sender queue for HL7v2.
    /// </summary>
    public class BackgroundMessageSender : BackgroundTransportBase, IDisposable
    {
        /// <summary>
        ///     Create a sender, which will monitor the queue and perform POSTs to the target address with messages in it.
        /// </summary>
        /// <param name="address"></param>
        public BackgroundMessageSender(string address)
        {
            var config = new MessageSenderConfiguration
            {
                Address = address,
            };

            Thread = new Thread(BackgroundMessageThreadMain);
            Thread.Start(config);
        }

        /// <summary>
        ///     If true, Dispose() has been called on this sender.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        ///     Stop sending and clean up.
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
        ///     Event that is invoked whenever a message is accepted as valid by the receiver.
        /// </summary>
        public event MessageTransportEventHandler MessageAccepted;

        /// <summary>
        ///     Event that is invoked whenever a message is rejected by the receiver.
        /// </summary>
        public event MessageTransportEventHandler MessageRejected;

        /// <summary>
        ///     Event that is invoked whenever a message is sent.
        /// </summary>
        public event MessageTransportEventHandler MessageSent;

        /// <summary>
        ///     Main method for the sender. This runs on a separate thread.
        /// </summary>
        /// <param name="configObject">Sender configuration.</param>
        private void BackgroundMessageThreadMain(object configObject)
        {
            var config = (MessageSenderConfiguration) configObject;

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
                        request.ContentType = Hl7ContentType;

                        using (var requestStream = request.GetRequestStream())
                        using (var messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message.Contents.ToString()))
                            )
                        {
                            messageStream.CopyTo(requestStream);
                            if (MessageSent != null)
                            {
                                MessageSent(this, new MessageTransportEventArgs(message.Contents.ToString(), null));
                            }
                        }

                        var response = request.GetResponse();
                        using (var responseStream = response.GetResponseStream())
                        using (var messageStream = new MemoryStream())
                        {
                            if (responseStream == null)
                            {
                                break;
                            }

                            responseStream.CopyTo(messageStream);
                            messageStream.Position = 0;

                            var messageReader = new MessageStreamReader(messageStream);
                            var responseMessage = messageReader.Read();
                            var responseMsa = responseMessage["MSA"].FirstOrDefault();

                            if (responseMsa == null)
                            {
                                break;
                            }

                            switch (responseMsa[1].Value)
                            {
                                case "AA":
                                    if (MessageAccepted != null)
                                    {
                                        MessageAccepted(this,
                                            new MessageTransportEventArgs(message.Contents, responseMessage));
                                    }
                                    break;
                                case "AR":
                                case "AE":
                                    if (MessageRejected != null)
                                    {
                                        MessageRejected(this,
                                            new MessageTransportEventArgs(message.Contents, responseMessage));
                                    }
                                    break;
                            }

                            if (responseMsa[1].Value != "AA")
                            {
                                Retry(message);
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
    }
}