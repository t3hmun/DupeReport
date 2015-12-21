namespace t3hmun.app.DupeReport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var delete = false;
            if (args.Contains("-d")) delete = true;

            var cwd = Directory.GetCurrentDirectory();
            var di = new DirectoryInfo(cwd);
            var res = Dupe.FindDupesInDir(di);
            var text = new List<string>();

            foreach (var group in res.Select(a => a.ToArray()))
            {
                IEnumerable<string> pathlines = null;
                string names = null;
                if (delete)
                {
                    var lensort = group.OrderBy(a => a.File.Name.Length).ToArray();
                    foreach (var f in lensort.Skip(1).Select(a => a.File))
                    {
                        //f.Delete();
                    }
                    var first = new[] { " " + lensort.First().File.FullName };
                    names = string.Join("|", lensort.Select(a => a.File.Name));
                    pathlines = first.Concat(lensort.Skip(1).Select(f => " DELETED " + f.File.FullName));
                }

                names = names ?? string.Join("|", group.Select(a => a.File.Name));
                pathlines = pathlines ?? group.Select(f => " " + f.File.FullName);
                text.Add(names);
                text.AddRange(pathlines);
            }

            File.WriteAllLines("dupes.txt", text);
        }
    }
}