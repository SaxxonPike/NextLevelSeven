using System.IO;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     A writer that wraps messages in MLP format.
    /// </summary>
    public class MlpStreamWriter : MessageStreamWriter
    {
        /// <summary>
        ///     Create an MLP stream writer that uses the specified stream as a destination.
        /// </summary>
        /// <param name="baseStream">Stream to write to.</param>
        public MlpStreamWriter(Stream baseStream) : base(baseStream)
        {
        }

        /// <summary>
        ///     Wrap raw data in an MLP packet.
        /// </summary>
        /// <param name="data">Data to wrap.</param>
        /// <returns>MLP-wrapped packet.</returns>
        protected override Stream Process(byte[] data)
        {
            var mem = new MemoryStream();
            mem.WriteByte(0x0B);
            mem.Write(data, 0, data.Length);
            mem.WriteByte(0x1C);
            mem.WriteByte(0x0D);
            mem.Position = 0;
            return mem;
        }
    }
}