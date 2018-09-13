using System;
using System.Collections.Generic;
using System.Linq;

namespace DocXcoDe
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var dict = ParseArgs(args);
            try
            {
                new DocumentProcessor(dict["XMLTEMPLATE"], dict["DOCXTEMPLATE"], dict["CONNECTIONSTRING"], dict["OUTFILENAME"])
                    .Process();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static Dictionary<string, string> ParseArgs(IEnumerable<string> args)
        {
            return args
                .Distinct()
                .Select(arg => arg.Split('='))
                .ToDictionary(pair => pair[0].ToUpperInvariant(),
                    pair => pair.Length > 1 ? string.Join("=", pair.Skip(1)) : "");
        }
    }
}
