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
    /// A threaded HTTP listener queue for HL7v2 messages.
    /// </summary>
    public class BackgroundMessageReceiver : MessageQueue, IDisposable
    {
        /// <summary>
        /// Create a receiver and begin listening on the specified port for HL7v2 over HTTP requests.
        /// </summary>
        /// <param name="port">Port number to listen on.</param>
        public BackgroundMessageReceiver(int port)
        {
            var config = new MessageReceiverConfiguration();
            config.Port = port;

            Thread = new Thread(BackgroundMessageThreadMain);
            Thread.Start(config);
        }

        /// <summary>
        /// Main method for the receiver. This runs on a separate thread.
        /// </summary>
        /// <param name="configObject">Receiver configuration.</param>
        void BackgroundMessageThreadMain(object configObject)
        {
            var config = (MessageReceiverConfiguration)configObject;
            var listener = new HttpListener();
            Listener = listener;
            var innerQueue = new List<IMessage>();

            try
            {
                listener.Prefixes.Add("http://*:" + config.Port + "/");
                listener.Start();

                while (!Disposed && !Aborted)
                {
                    Ready = true;
                    var context = listener.GetContext();
                    Ready = false;
                    var httpRequest = context.Request;
                    var httpResponse = context.Response;

                    IMessage request = null;

                    using (var mem = new MemoryStream())
                    {
                        httpRequest.InputStream.CopyTo(mem);
                        mem.Position = 0;
                        while (true)
                        {
                            var message = ReadMessageStream(mem);
                            if (request == null)
                            {
                                request = message;
                            }

                            if (message == null)
                            {
                                break;
                            }

                            innerQueue.Add(message);
                        }
                    }

                    IMessage responseMessage;
                    if (request != null)
                    {
                        responseMessage = AckMessageGenerator.GenerateSuccess(request);
                    }
                    else
                    {
                        responseMessage = AckMessageGenerator.GenerateReject(new Message());
                    }

                    httpResponse.ContentType = "x-application/hl7-v2+er7";
                    using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(responseMessage.ToString())))
                    {
                        mem.CopyTo(httpResponse.OutputStream);
                        httpResponse.OutputStream.Close();
                    }

                    foreach (var message in innerQueue)
                    {
                        Enqueue(message);
                    }

                    innerQueue.Clear();
                }
            }
            catch (ThreadAbortException)
            {
                Aborted = true;
            }
            catch (HttpListenerException)
            {
                Aborted = true;
            }
            finally
            {
                if (Listener != null)
                {
                    Listener.Close();                    
                }
            }
        }

        /// <summary>
        /// If true, the thread was aborted.
        /// </summary>
        bool Aborted
        {
            get;
            set;
        }

        /// <summary>
        /// Internally used listener object.
        /// </summary>
        HttpListener Listener
        {
            get;
            set;
        }

        /// <summary>
        /// Read an HL7v2 message from a stream.
        /// </summary>
        /// <param name="source">Stream to read from.</param>
        /// <returns>Loaded message.</returns>
        static IMessage ReadMessageStream(Stream source)
        {
            var reader = new HL7StreamReader(source);
            var message = reader.Read();
            return message;
        }

        /// <summary>
        /// If true, the receiver is ready to process further requests. If false, the receiver is busy processing requests.
        /// </summary>
        public bool Ready
        {
            get;
            private set;
        }

        /// <summary>
        /// Thread the receiver's main method is running on.
        /// </summary>
        Thread Thread
        {
            get;
            set;
        }

        /// <summary>
        /// Stop listening, close the port, and clean up.
        /// </summary>
        public void Dispose()
        {
            if (Thread != null)
            {
                Ready = false;
                Disposed = true;
                if (Listener != null)
                {
                    var listener = Listener;
                    Listener = null;
                    listener.Close();                    
                }
                Thread = null;
            }
        }

        /// <summary>
        /// If true, Dispose() has been called on this receiver.
        /// </summary>
        public bool Disposed
        {
            get;
            private set;
        }
    }
}
