using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     Base class for background senders and receivers. This is an abstract class.
    /// </summary>
    public abstract class BackgroundTransportBase : MessageQueue, IDisposable
    {
        public static readonly string Hl7ContentType = "x-application/hl7-v2+er7";

        /// <summary>
        ///     If true, the thread was aborted.
        /// </summary>
        protected bool Aborted { get; set; }

        /// <summary>
        ///     Cancellation token for the task.
        /// </summary>
        private CancellationTokenSource CancelTokenSource { get; set; }

        /// <summary>
        ///     If true, Dispose() has been called on this transport.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        ///     If true, the transport is ready to process further requests. If false, the transport is busy processing requests.
        /// </summary>
        public bool Ready { get; protected set; }

        /// <summary>
        ///     If true, transport is active. If false, the transport is not processing requests.
        /// </summary>
        public bool Running { get; protected set; }

        /// <summary>
        ///     Thread the main method is running on.
        /// </summary>
        protected Task Task { get; set; }

        /// <summary>
        ///     Stop transport and clean up.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Finalizer.
        /// </summary>
        ~BackgroundTransportBase()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Background message thread main method.
        /// </summary>
        protected abstract void BackgroundMessageThreadMain();

        /// <summary>
        ///     Background message thread exception handler.
        /// </summary>
        /// <param name="task">Running task.</param>
        protected virtual void BackgroundMessageThreadExceptionHandler(Task task)
        {
            if (task.Exception != null)
            {
                throw task.Exception;
            }
        }

        /// <summary>
        ///     Clean up native resources.
        /// </summary>
        /// <param name="disposeAll">If true, clean up managed resources also.</param>
        protected virtual void Dispose(bool disposeAll)
        {
            if (!disposeAll) return;
            if (Task != null)
            {
                Stop();
                Task = null;
            }
            Disposed = true;
        }

        /// <summary>
        ///     Start processing messages on the transport.
        /// </summary>
        public virtual void Start()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (Running)
            {
                return;
            }

            CancelTokenSource = new CancellationTokenSource();
            var cancelToken = CancelTokenSource.Token;
            Task = new Task(BackgroundMessageThreadMain, cancelToken);
            Task.ContinueWith(BackgroundMessageThreadExceptionHandler, cancelToken);
            Task.Start();
            Running = true;
        }

        /// <summary>
        ///     Stop processing messages on the transport.
        /// </summary>
        public virtual void Stop()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (!Running)
            {
                return;
            }

            Running = false;
            Ready = false;
            CancelTokenSource.Cancel();
        }

        /// <summary>
        ///     Wait for the transport to become ready. An exception is thrown if this is not done before the timeout specified.
        /// </summary>
        /// <param name="timeoutMilliseconds">Timeout of the wait in milliseconds.</param>
        public void WaitToBeReady(int timeoutMilliseconds = 10000)
        {
            if (!Running)
            {
                throw new MessageTransportException(ErrorCode.TransportNotStarted);
            }

            var sw = new Stopwatch();
            sw.Start();

            while (!Ready)
            {
                Thread.Sleep(100);
                if (sw.ElapsedMilliseconds > timeoutMilliseconds)
                {
                    throw new TimeoutException(ErrorMessages.Get(ErrorCode.TimedOutWaitingForTransportToBecomeReady));
                }
            }
        }
    }
}