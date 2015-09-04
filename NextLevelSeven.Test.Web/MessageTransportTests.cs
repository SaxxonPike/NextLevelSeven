using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Web;

namespace NextLevelSeven.Test.Web
{
    [TestClass]
    public class MessageTransportTests
    {
        [TestMethod]
        public void MessageTransport_CanSendAndReceiveMessages()
        {
            var port = Randomized.Number(50000, 64000);
            BackgroundMessageReceiver receiver = null;
            BackgroundMessageSender sender = null;

            Debug.WriteLine("Building sender and receiver...");
            Measure.ExecutionTime(() =>
            {
                receiver = new BackgroundMessageReceiver(port);
                sender = new BackgroundMessageSender("http://localhost:" + port + "/");
            });

            Debug.WriteLine("Spinning up receiver...");
            Measure.ExecutionTime(receiver.Start);

            Debug.WriteLine("Spinning up sender...");
            Measure.ExecutionTime(sender.Start);

            Assert.AreEqual(0, receiver.Count, "Receiver queue is not empty.");
            Assert.AreEqual(0, sender.Count, "Sender queue is not empty.");

            Debug.WriteLine("Enqueueing message...");
            sender.Enqueue(new Message(ExampleMessages.Standard));
            Measure.WaitTime(() =>
            {
                var queueIsPopulated = receiver.Count > 0;
                var receiverReady = receiver.Ready;
                var senderReady = sender.Ready;

                return queueIsPopulated && receiverReady && senderReady;
            }, long.MaxValue, "Receiver didn't receive message.");

            Assert.AreEqual(1, receiver.Count, "Receiver queue doesn't have exactly one message after transmission.");
            Assert.AreEqual(0, sender.Count, "Sender queue is not empty after transmission.");

            Debug.WriteLine("Tearing down...");
            receiver.Stop();
            sender.Stop();
            receiver.Dispose();
            sender.Dispose();
        }
    }
}
