namespace t3hmun.Datos
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// Finding duplicate files.
    /// </summary>
    public class Dupe
    {
        /// <summary>
        /// Returns groups of files that have other files with identical SHA512 hashes.
        /// </summary>
        /// <param name="dir">Dir to search under.</param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<string, HashFileTuple>> FindDupesInDir(DirectoryInfo dir)
        {
            var files = GetAllFiles(dir);
            var sizeDupes = files.GroupBy(f => f.Length).Where(g => g.Count() > 1);

            // Flatten and then group again is easier than checking for dupes in the groups...
            var hashes = CalcHashesAndFlatten(sizeDupes);

            var results = hashes.GroupBy(h => h.Hash).Where(g => g.Count() > 1);
            return results;
        }

        /// <summary>
        /// Reads files, calcs hashes, puts in object, returns list.
        /// </summary>
        private static List<HashFileTuple> CalcHashesAndFlatten(IEnumerable<IGrouping<long, FileInfo>> sizeDupes)
        {
            var hasher = new SHA512Managed();
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
            return hashes;
        }

        /// <summary>
        /// Builds enumerable of all files in dir and subdir, recursive.
        /// </summary>
        /// <param name="dir">Base directory.</param>
        /// <returns>An enumerable of all the files in the directory and subdirectories.</returns>
        private static IEnumerable<FileInfo> GetAllFiles(DirectoryInfo dir)
        {
            IEnumerable<FileInfo> files = dir.GetFiles();
            foreach (var d in dir.GetDirectories())
            {
                files = files.Concat(GetAllFiles(d));
            }
            return files;
        }

        /// <summary>
        /// Tuple of hash as string and FileInfo.
        /// </summary>
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