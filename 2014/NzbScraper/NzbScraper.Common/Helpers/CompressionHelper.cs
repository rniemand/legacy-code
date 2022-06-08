using System.IO;
using System.IO.Compression;

namespace NzbScraper.Common.Helpers
{
    public static class CompressionHelper
    {
        public static byte[] Decompress(byte[] gzip)
        {
            // http://www.dotnetperls.com/decompress
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];

                using (var memory = new MemoryStream())
                {
                    var count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
