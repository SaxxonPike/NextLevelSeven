using System;
using System.Diagnostics;
using System.Threading;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     Base class for background senders and receivers. This is an abstract class.
    /// </summary>
    public abstract class BackgroundTransportBase : MessageQueue
    {
        public static readonly string Hl7ContentType = "x-application/hl7-v2+er7";

        /// <summary>
        ///     If true, the thread was aborted.
        /// </summary>
        protected bool Aborted { get; set; }

        /// <summary>
        ///     If true, the sender is ready to process further requests. If false, the sender is busy processing requests.
        /// </summary>
        public bool Ready { get; protected set; }

        /// <summary>
        ///     Thread the main method is running on.
        /// </summary>
        protected Thread Thread { get; set; }

        /// <summary>
        ///     Wait for the transport to become ready. An exception is thrown if this is not done before the timeout specified.
        /// </summary>
        /// <param name="timeoutMilliseconds">Timeout of the wait in milliseconds.</param>
        public void WaitToBeReady(int timeoutMilliseconds = 10000)
        {
            var sw = new Stopwatch();
            sw.Start();

            while (!Ready)
            {
                Thread.Sleep(1);
                if (sw.ElapsedMilliseconds > timeoutMilliseconds)
                {
                    throw new TimeoutException(ErrorMessages.Get(ErrorCode.TimedOutWaitingForTransportToBecomeReady));
                }
            }
        }
    }
}