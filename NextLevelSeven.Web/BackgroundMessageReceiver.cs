using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using NextLevelSeven.Core;
using NextLevelSeven.Generation;
using NextLevelSeven.Native;
using NextLevelSeven.Streaming;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     A threaded HTTP listener queue for HL7v2 messages.
    /// </summary>
    public class BackgroundMessageReceiver : BackgroundTransportBase
    {
        /// <summary>
        ///     Receiver configuration.
        /// </summary>
        public readonly MessageReceiverConfiguration Configuration;

        /// <summary>
        ///     Create a receiver and begin listening on the specified port for HL7v2 over HTTP requests. Facility and Application
        ///     fields are automatically populated.
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
            Configuration = config;
        }

        /// <summary>
        ///     Create a receiver and begin listening on the specified port for HL7v2 over HTTP requests.
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
            Configuration = config;
        }

        /// <summary>
        ///     Internally used listener object.
        /// </summary>
        private HttpListener Listener { get; set; }

        /// <summary>
        ///     Event that is invoked whenever a message is accepted as valid.
        /// </summary>
        public event MessageTransportEventHandler MessageAccepted;

        /// <summary>
        ///     Event that is invoked whenever a message is received.
        /// </summary>
        public event MessageTransportEventHandler MessageReceived;

        /// <summary>
        ///     Event that is invoked whenever a message is rejected.
        /// </summary>
        public event MessageTransportEventHandler MessageRejected;

        /// <summary>
        ///     Main method for the receiver. This runs on a separate thread.
        /// </summary>
        protected override async void BackgroundMessageThreadMain()
        {
            var config = Configuration;
            var listener = new HttpListener();
            Listener = listener;
            var innerQueue = new List<IMessageParser>();

            try
            {
                listener.Prefixes.Add("http://*:" + config.Port + "/");
                listener.Start();

                while (!Disposed && !Aborted)
                {
                    Thread.Sleep(1);
                    Ready = true;
                    var context = await listener.GetContextAsync();
                    Ready = false;
                    var httpRequest = context.Request;
                    var httpResponse = context.Response;
                    var failureReason = string.Empty;
                    var messageRawData = new byte[0];

                    IMessageParser request = null;

                    using (var mem = new MemoryStream())
                    {
                        httpRequest.InputStream.CopyTo(mem);
                        mem.Position = 0;
                        while (true)
                        {
                            IMessageParser message;

                            try
                            {
                                message = ReadMessageStream(mem);
                                if (message != null && MessageReceived != null)
                                {
                                    MessageReceived(this, new MessageTransportEventArgs(null, message.Value));
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

                            if (config.ReceivedMessageRouter == null || !config.ReceivedMessageRouter.Route(message))
                            {
                                innerQueue.Add(message);
                            }
                        }
                    }

                    IMessageParser responseMessage;
                    if (request != null)
                    {
                        responseMessage = AckMessageGenerator.GenerateSuccess(request, null, config.OwnFacility,
                            config.OwnApplication);
                        if (MessageAccepted != null)
                        {
                            MessageAccepted(this,
                                new MessageTransportEventArgs(responseMessage.Value, request.Value));
                        }
                    }
                    else
                    {
                        responseMessage = AckMessageGenerator.GenerateReject(Message.Parse(), failureReason,
                            config.OwnFacility, config.OwnApplication);
                        if (MessageRejected != null)
                        {
                            MessageRejected(this,
                                new MessageTransportEventArgs(responseMessage.Value,
                                    Encoding.UTF8.GetString(messageRawData)));
                        }
                    }

                    httpResponse.ContentType = Hl7ContentType;
                    using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(responseMessage.Value)))
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
            catch (OperationCanceledException)
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
        ///     Read an HL7v2 message from a stream.
        /// </summary>
        /// <param name="source">Stream to read from.</param>
        /// <returns>Loaded message.</returns>
        private static IMessageParser ReadMessageStream(Stream source)
        {
            var reader = new MessageStreamReader(source);
            var message = reader.Read();
            return message;
        }
    }
}