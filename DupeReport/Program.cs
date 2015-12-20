namespace t3hmun.app.DupeReport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var cwd = Directory.GetCurrentDirectory();
            var di = new DirectoryInfo(cwd);
            var res = Dupe.FindDupesInDir(di);
            var text = new List<string>();
            foreach (var group in res)
            {
                var names = string.Join("|", group.Select(a => a.File.Name));
                var pathlines = group.Select(f => " " + f.File.FullName);
                text.Add(names);
                text.AddRange(pathlines);
            }

            File.WriteAllLines("dupes.txt", text);
        }
    }
}