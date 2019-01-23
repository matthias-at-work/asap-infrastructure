using System;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;

namespace Packaging
{
    public static class Tarball
    {
        public static void Pack(string[] filesToInclude, string tarFilename)
        {
            using (FileStream outStream = File.Create(tarFilename))
            {
                TarArchive archive = TarArchive.CreateOutputTarArchive(outStream);

                foreach (string file in filesToInclude)
                {
                    TarEntry entry = TarEntry.CreateEntryFromFile(file);
                    archive.WriteEntry(entry, true);
                }

                archive.Close();
            }
        }

        public static void Unpack(string tarFilename, string destinationDirectory)
        {
            FileStream stream = File.OpenRead(tarFilename);
            TarArchive tarArchive = TarArchive.CreateInputTarArchive(stream);
            tarArchive.ExtractContents(destinationDirectory);
            tarArchive.Close();
            stream.Close();
        }
    }
}
