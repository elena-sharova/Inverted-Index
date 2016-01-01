using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InvertedIndex
{

    public static class Extensions
    {
        public static IEnumerable<string> And (this Dictionary<string, List<string>> index, string firstTerm, string secondTerm)
        {
            return(from d in index
                         where d.Key.Equals(firstTerm)
                         select d.Value).SelectMany(x=>x).Intersect
                            ((from d in index
                             where d.Key.Equals(secondTerm)
                             select d.Value).SelectMany(x => x));
        }

        public static IEnumerable<string> Or(this Dictionary<string, List<string>> index, string firstTerm, string secondTerm)
        {
            //return (from d in index
            //        where d.Key.Equals(firstTerm)
            //        select d.Value).SelectMany(x => x).ToList().Union
            //                ((from d in index
            //                  where d.Key.Equals(secondTerm)
            //                  select d.Value).SelectMany(x => x).ToList()).Distinct();

            return (from d in index
                        where d.Key.Equals(firstTerm) || d.Key.Equals(secondTerm)
                        select d.Value).SelectMany(x=>x).Distinct();
        }

    }

    class EntryPoint
    {
        public static Dictionary<string, List<string>> invertedIndex;
 
        static void Main(string[] args)
        {
            invertedIndex = new Dictionary<string, List<string>>();
            string folder = "C:\\Users\\Elena\\Documents\\Visual Studio 2013\\Projects\\InvertedIndex\\Files\\";

            foreach (string file in Directory.EnumerateFiles(folder, "*.txt"))
            {
                List<string> content = System.IO.File.ReadAllText(file).Split(' ').Distinct().ToList();
                addToIndex(content, file.Replace(folder, ""));
            }

            var resAnd = invertedIndex.And("star", "sparkling");
            var resOr = invertedIndex.Or("star", "sparkling");

            Console.ReadLine();
        }

        private static void addToIndex(List<string> words, string document)
        {
            foreach (var word in words)
            {
                if (!invertedIndex.ContainsKey(word))
                {
                    invertedIndex.Add(word, new List<string> { document });
                }
                else
                {
                    invertedIndex[word].Add(document);
                }
            }
        }
    }
}
