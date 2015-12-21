﻿namespace t3hmun.app.DupeReport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var simulatedelete = false;
            var delete = false;
            var spam = true;
            if (args.Any())
            {
                if(args.Contains("-s"))
                {
                    spam = false;
                }

                if (args.Contains("-x"))
                {
                    if (spam) Spam("Simulate delete mode, files will not be deleted, only logged as deleted.");
                    simulatedelete = true;
                    // Simulate mode overrides delete mode.
                }
                else if (args.Contains("-d"))
                {
                    if (spam) Spam("Delete mode, only the shortest name will be kept. If length is identical delete is random.");
                    delete = true;
                }
            }

            var cwd = Directory.GetCurrentDirectory();
            var di = new DirectoryInfo(cwd);
            var res = Dupe.FindDupesInDir(di, spam);
            var text = new List<string>();

            if (spam) Spam("Generating report...");
            foreach (var group in res.Select(a => a.ToArray()))
            {
                IEnumerable<string> pathlines = null;
                string names = null;
                if (delete || simulatedelete)
                {
                    var lensort = group.OrderBy(a => a.File.Name.Length).ToArray();
                    if (delete)
                    {
                        foreach (var f in lensort.Skip(1).Select(a => a.File))
                        {
                            f.Delete();
                        }
                    }
                    var first = new[] { "     " + lensort.First().File.FullName };
                    names = string.Join("|", lensort.Select(a => a.File.Name));
                    pathlines = first.Concat(lensort.Skip(1).Select(f => " DEL " + f.File.FullName));
                }

                names = names ?? string.Join("|", group.Select(a => a.File.Name));
                pathlines = pathlines ?? group.Select(f => " " + f.File.FullName);
                text.Add(names);
                text.AddRange(pathlines);
            }
            if (spam) Spam("... done.");

            if (spam) Spam("Writing Report...");
            File.WriteAllLines("dupes.txt", text);
            if (spam) Spam("... done.");
        }

        /// <summary>
        /// Wrapping Console.WriteLine makes it easier to implement other logging modes later.
        /// </summary>
        private static void Spam (string text)
        {
            Console.WriteLine(text);
        }
    }
}