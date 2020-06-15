namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System.IO;
    using System.IO.Compression;

    public static class ByteExtensions
    {
        /// <summary>
        ///     Compresses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] data)
        {
            byte[] outputArray;

            using (var output = new MemoryStream())
            {
                using (var dstream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    dstream.Write(data, 0, data.Length);
                }

                outputArray = output.ToArray();
            }

            return outputArray;
        }

        /// <summary>
        ///     Decompresses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] data)
        {
            byte[] outputArray;

            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (var dstream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        dstream.CopyTo(output);
                    }

                    outputArray = output.ToArray();
                }
            }

            return outputArray;
        }
    }
}