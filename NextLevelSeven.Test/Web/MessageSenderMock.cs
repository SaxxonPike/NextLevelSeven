using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Web;

namespace NextLevelSeven.Test.Web
{
    static public class MessageSenderMock
    {
        private const int TransportPort = 49999;

        /// <summary>
        /// Build a receiver, send raw HL7 data to it, and return the response.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="type">MIME type, defaults to HL7-ER7.</param>
        /// <returns>Response from the receiver.</returns>
        static public string SendData(string data, string type = "x-application/hl7-v2+er7")
        {
            using (var receiver = new BackgroundMessageReceiver(TransportPort))
            {
                receiver.Start();

                var request = WebRequest.Create("http://localhost:" + TransportPort + "/");
                request.ContentType = type;
                request.Method = "POST";

                Debug.WriteLine("Sending message via mock:");
                Debug.WriteLine(data);
                Debug.WriteLine(string.Empty);
                using (var requestStream = request.GetRequestStream())
                using (var requestData = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    requestData.CopyTo(requestStream);
                }

                Debug.WriteLine("Receiving message via mock:");
                using (var responseStream = request.GetResponse().GetResponseStream())
                using (var responseData = new MemoryStream())
                {
                    if (responseStream != null)
                    {
                        responseStream.CopyTo(responseData);
                        var responseString = Encoding.UTF8.GetString(responseData.ToArray());
                        Debug.WriteLine(responseString);
                        return responseString;
                    }
                }
            }

            Debug.WriteLine("(failed.)");
            return null;
        }

        /// <summary>
        /// Build a receiver, send a message to it, and return the response.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <returns>Response from the receiver.</returns>
        static public string SendMessage(IMessage message)
        {
            return SendData(message.ToString());
        }

        /// <summary>
        /// Build a receiver, send plain text to it, and return the response.
        /// </summary>
        /// <param name="text">Text to send.</param>
        /// <returns>Response from the receiver.</returns>
        static public string SendPlain(string text)
        {
            return SendData(text, "text/plain");
        }
    }
}
