using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holo.Zip
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ZipFileExtensions
    {
        public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName)
        {
            return ZipFileExtensions.DoCreateEntryFromFile(destination, sourceFileName, entryName, null);
        }

        public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionLevel compressionLevel)
        {
            return ZipFileExtensions.DoCreateEntryFromFile(destination, sourceFileName, entryName, new CompressionLevel?(compressionLevel));
        }

        public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (destinationDirectoryName == null)
            {
                throw new ArgumentNullException("destinationDirectoryName");
            }
            DirectoryInfo directoryInfo = Directory.CreateDirectory(destinationDirectoryName);
            string text = directoryInfo.FullName;
            if (!LocalAppContextSwitches.DoNotAddTrailingSeparator)
            {
                int length = text.Length;
                if (length != 0 && text[length - 1] != Path.DirectorySeparatorChar)
                {
                    text += Path.DirectorySeparatorChar.ToString();
                }
            }
            foreach (ZipArchiveEntry current in source.Entries)
            {
                string fullPath = Path.GetFullPath(Path.Combine(text, current.FullName));
                if (!fullPath.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("error");
                }
                if (Path.GetFileName(fullPath).Length == 0)
                {
                    if (current.Length != 0L)
                    {
                        throw new IOException("error");
                    }
                    Directory.CreateDirectory(fullPath);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    current.ExtractToFile(fullPath, false);
                }
            }
        }

        internal static ZipArchiveEntry DoCreateEntryFromFile(ZipArchive destination, string sourceFileName, string entryName, CompressionLevel? compressionLevel)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName");
            }
            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }
            ZipArchiveEntry result;
            using (Stream stream = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ZipArchiveEntry zipArchiveEntry = compressionLevel.HasValue ? destination.CreateEntry(entryName, compressionLevel.Value) : destination.CreateEntry(entryName);
                DateTime lastWriteTime = File.GetLastWriteTime(sourceFileName);
                if (lastWriteTime.Year < 1980 || lastWriteTime.Year > 2107)
                {
                    lastWriteTime = new DateTime(1980, 1, 1, 0, 0, 0);
                }
                zipArchiveEntry.LastWriteTime = lastWriteTime;
                using (Stream stream2 = zipArchiveEntry.Open())
                {
                    stream.CopyTo(stream2);
                }
                result = zipArchiveEntry;
            }
            return result;
        }

        public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName)
        {
            source.ExtractToFile(destinationFileName, false);
        }

        public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName, bool overwrite)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (destinationFileName == null)
            {
                throw new ArgumentNullException("destinationFileName");
            }
            FileMode mode = overwrite ? FileMode.Create : FileMode.CreateNew;
            using (Stream stream = File.Open(destinationFileName, mode, FileAccess.Write, FileShare.None))
            {
                using (Stream stream2 = source.Open())
                {
                    stream2.CopyTo(stream);
                }
            }
            File.SetLastWriteTime(destinationFileName, source.LastWriteTime.DateTime);
        }
    }
}
