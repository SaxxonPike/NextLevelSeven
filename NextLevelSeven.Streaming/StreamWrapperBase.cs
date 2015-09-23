using System.IO;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     Base class for HL7 stream readers and writers. This is an abstract class.
    /// </summary>
    public abstract class StreamWrapperBase
    {
        /// <summary>
        ///     Initialize the BaseStream property.
        /// </summary>
        /// <param name="baseStream">Stream to use for the BaseStream property.</param>
        protected StreamWrapperBase(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        /// <summary>
        ///     The stream that will be accessed for reader and writer functionality.
        /// </summary>
        public Stream BaseStream { get; private set; }
    }
}