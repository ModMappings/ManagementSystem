using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Data.MCP.TSRG.Importer.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static IEnumerable<string> ReadAllLines(
            this ZipArchive archive,
            string pathInZip,
            Encoding encoding)
        {
            var zipEntry = archive.GetEntry(pathInZip);
            if (zipEntry == null)
                throw new ArgumentOutOfRangeException(nameof(pathInZip), "The archive does not contain the given path.");

            using (var stream = zipEntry.Open())
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
