using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace lab4.Implementation
{
    public static class ZipHelper
    {
        public static Dictionary<string, byte[]> Unzip(byte[] zippedBuffer)
        {
            using (var zippedStream = new MemoryStream(zippedBuffer))
            {
                using (var archive = new ZipArchive(zippedStream))
                {
                    var allEntries = new Dictionary<string, byte[]>();
                    
                    foreach (var entry in archive.Entries)
                    {
                        if (entry != null)
                        {
                            using (var unzippedEntryStream = entry.Open())
                            {
                                using (var ms = new MemoryStream())
                                {
                                    unzippedEntryStream.CopyTo(ms);
                                    var unzippedArray = ms.ToArray();

                                    allEntries[entry.Name] = unzippedArray; //Encoding.Default.GetString(unzippedArray);
                                }
                            }
                        }
                    }
                    
                    return allEntries;
                }
            }
        }
    }
}