using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holo.Zip
{
    public static class ZipFile
    {
        private static readonly char s_pathSeperator = LocalAppContextSwitches.ZipFileUseBackslash ? '\\' : '/';

        public static ZipArchive OpenRead(string archiveFileName)
        {
            return ZipFile.Open(archiveFileName, ZipArchiveMode.Read);
        }

        public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode)
        {
            return ZipFile.Open(archiveFileName, mode, null);
        }

        public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode, Encoding entryNameEncoding)
        {
            FileMode mode2;
            FileAccess access;
            FileShare share;
            switch (mode)
            {
                case ZipArchiveMode.Read:
                    mode2 = FileMode.Open;
                    access = FileAccess.Read;
                    share = FileShare.Read;
                    break;
                case ZipArchiveMode.Create:
                    mode2 = FileMode.CreateNew;
                    access = FileAccess.Write;
                    share = FileShare.None;
                    break;
                case ZipArchiveMode.Update:
                    mode2 = FileMode.OpenOrCreate;
                    access = FileAccess.ReadWrite;
                    share = FileShare.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            FileStream fileStream = null;
            ZipArchive result;
            try
            {
                fileStream = File.Open(archiveFileName, mode2, access, share);
                result = new ZipArchive(fileStream, mode, false, entryNameEncoding);
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
                throw;
            }
            return result;
        }

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
        {
            ZipFile.DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, null, false, null);
        }

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            ZipFile.DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, new CompressionLevel?(compressionLevel), includeBaseDirectory, null);
        }

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            ZipFile.DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, new CompressionLevel?(compressionLevel), includeBaseDirectory, entryNameEncoding);
        }

        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
        {
            ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, null);
        }

        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Encoding entryNameEncoding)
        {
            if (sourceArchiveFileName == null)
            {
                throw new ArgumentNullException("sourceArchiveFileName");
            }
            using (ZipArchive zipArchive = ZipFile.Open(sourceArchiveFileName, ZipArchiveMode.Read, entryNameEncoding))
            {
                zipArchive.ExtractToDirectory(destinationDirectoryName);
            }
        }

        private static void DoCreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel? compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            sourceDirectoryName = Path.GetFullPath(sourceDirectoryName);
            destinationArchiveFileName = Path.GetFullPath(destinationArchiveFileName);
            using (ZipArchive zipArchive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Create, entryNameEncoding))
            {
                bool flag = true;
                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectoryName);
                string fullName = directoryInfo.FullName;
                if (includeBaseDirectory && directoryInfo.Parent != null)
                {
                    fullName = directoryInfo.Parent.FullName;
                }
                foreach (FileSystemInfo current in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                {
                    flag = false;
                    int length = current.FullName.Length - fullName.Length;
                    string text;
                    if (LocalAppContextSwitches.ZipFileUseBackslash)
                    {
                        text = current.FullName.Substring(fullName.Length, length);
                        text = text.TrimStart(new char[]
                        {
                            Path.DirectorySeparatorChar,
                            Path.AltDirectorySeparatorChar
                        });
                    }
                    else
                    {
                        text = ZipFile.EntryFromPath(current.FullName, fullName.Length, length);
                    }
                    if (current is FileInfo)
                    {
                        ZipFileExtensions.DoCreateEntryFromFile(zipArchive, current.FullName, text, compressionLevel);
                    }
                    else
                    {
                        if (current is DirectoryInfo directoryInfo2 && ZipFile.IsDirEmpty(directoryInfo2))
                        {
                            zipArchive.CreateEntry(text + ZipFile.s_pathSeperator.ToString());
                        }
                    }
                }
                if (includeBaseDirectory & flag)
                {
                    string text = LocalAppContextSwitches.ZipFileUseBackslash ? directoryInfo.Name : ZipFile.EntryFromPath(directoryInfo.Name, 0, directoryInfo.Name.Length);
                    zipArchive.CreateEntry(text + ZipFile.s_pathSeperator.ToString());
                }
            }
        }

        private static string EntryFromPath(string entry, int offset, int length)
        {
            while (length > 0 && (entry[offset] == Path.DirectorySeparatorChar || entry[offset] == Path.AltDirectorySeparatorChar))
            {
                offset++;
                length--;
            }
            if (length == 0)
            {
                return string.Empty;
            }
            char[] array = entry.ToCharArray(offset, length);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == Path.DirectorySeparatorChar || array[i] == Path.AltDirectorySeparatorChar)
                {
                    array[i] = ZipFile.s_pathSeperator;
                }
            }
            return new string(array);
        }

        private static bool IsDirEmpty(DirectoryInfo possiblyEmptyDir)
        {
            using (IEnumerator<FileSystemInfo> enumerator = possiblyEmptyDir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    FileSystemInfo current = enumerator.Current;
                    return false;
                }
            }
            return true;
        }
    }
}
