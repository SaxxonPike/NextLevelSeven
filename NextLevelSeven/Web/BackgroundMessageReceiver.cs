using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.MessageGeneration;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Web
{
    /// <summary>
    /// A threaded HTTP listener queue for HL7v2 messages.
    /// </summary>
    public class BackgroundMessageReceiver : BackgroundTransportBase, IDisposable
    {
        /// <summary>
        /// Create a receiver and begin listening on the specified port for HL7v2 over HTTP requests. Facility and Application fields are automatically populated.
        /// </summary>
        /// <param name="port"></param>
        public BackgroundMessageReceiver(int port)
        {
            var config = new MessageReceiverConfiguration
            {
                Port = port,
                OwnFacility = Environment.UserDomainName,
                OwnApplication = Process.GetCurrentProcess().ProcessName,
            };
            Initialize(config);
        }

        /// <summary>
        /// Create a receiver and begin listening on the specified port for HL7v2 over HTTP requests.
        /// </summary>
        /// <param name="port">Port number to listen on.</param>
        /// <param name="facility">Receiving facility.</param>
        /// <param name="application">Receiving application.</param>
        public BackgroundMessageReceiver(int port, string facility, string application)
        {
            var config = new MessageReceiverConfiguration
            {
                Port = port,
                OwnFacility = facility,
                OwnApplication = application,
            };
            Initialize(config);
        }

        /// <summary>
        /// Event that is invoked whenever a message is accepted as valid.
        /// </summary>
        public event MessageTransportEventHandler MessageAccepted;

        /// <summary>
        /// Event that is invoked whenever a message is received.
        /// </summary>
        public event MessageTransportEventHandler MessageReceived;

        /// <summary>
        /// Event that is invoked whenever a message is rejected.
        /// </summary>
        public event MessageTransportEventHandler MessageRejected;

        /// <summary>
        /// Initialize the reader with the specified config.
        /// </summary>
        /// <param name="config">Config to initialize with.</param>
        void Initialize(MessageReceiverConfiguration config)
        {
            Thread = new Thread(BackgroundMessageThreadMain);
            Thread.Start(config);
            WaitToBeReady();
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
                    var failureReason = string.Empty;
                    var messageRawData = new byte[0];

                    IMessage request = null;

                    using (var mem = new MemoryStream())
                    {
                        httpRequest.InputStream.CopyTo(mem);
                        mem.Position = 0;
                        while (true)
                        {
                            IMessage message;

                            try
                            {
                                message = ReadMessageStream(mem);
                                if (MessageReceived != null)
                                {
                                    MessageReceived(this, new MessageTransportEventArgs(null, message.ToString()));
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!(ex is MessageException || ex is ElementException))
                                {
                                    throw;
                                }

                                failureReason = ex.Message;
                                request = null;
                                messageRawData = mem.ToArray();
                                innerQueue.Clear();
                                break;
                            }

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
                        responseMessage = AckMessageGenerator.GenerateSuccess(request, null, config.OwnFacility, config.OwnApplication);
                        if (MessageAccepted != null)
                        {
                            MessageAccepted(this, new MessageTransportEventArgs(responseMessage.ToString(), request.ToString()));
                        }
                    }
                    else
                    {
                        responseMessage = AckMessageGenerator.GenerateReject(new Message(), failureReason, config.OwnFacility, config.OwnApplication);
                        if (MessageRejected != null)
                        {
                            MessageRejected(this, new MessageTransportEventArgs(responseMessage.ToString(), Encoding.UTF8.GetString(messageRawData)));
                        }
                    }

                    httpResponse.ContentType = Hl7ContentType;
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
        /// Stop listening, close the port, and clean up.
        /// </summary>
        public void Dispose()
        {
            if (Thread == null)
            {
                return;
            }

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
