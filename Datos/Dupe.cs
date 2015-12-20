namespace Datos
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    public class Dupe
    {
        public static IEnumerable<IGrouping<string, HashFileTuple>> FindDupesInDir(DirectoryInfo dir)
        {
            var files = GetAllFiles(dir);
            var sizeDupes = files.GroupBy(f => f.Length).Where(g => g.Count() > 1);
            var hasher = new SHA512Managed();

            // Tuples are less readable than anonymous types, wrting a class is just laborious.
            var hashes = new List<HashFileTuple>();

            foreach (var sizeDupeGroup in sizeDupes)
            {
                foreach (var fi in sizeDupeGroup)
                {
                    var data = File.ReadAllBytes(fi.FullName);
                    var hashBytes = hasher.ComputeHash(data);
                    // Can't group arrays without a custom IEqualityComparer.
                    // Also want a string for humans anyway.
                    hashes.Add(new HashFileTuple(BitConverter.ToString(hashBytes), fi));
                }
            }

            var results = hashes.GroupBy(h => h.Hash).Where(g => g.Count() > 1);
            return results;
        }

        private static IEnumerable<FileInfo> GetAllFiles(DirectoryInfo dir)
        {
            throw new NotImplementedException();
        }

        public class HashFileTuple
        {
            public HashFileTuple(string hash, FileInfo file)
            {
                Hash = hash;
                File = file;
            }

            public FileInfo File { get; private set; }
            public string Hash { get; private set; }
        }
    }
}