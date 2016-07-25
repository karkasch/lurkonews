using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lurkonews.Domain
{
    public class Lurkdict
    {
        public static Random Rnd; 
        static Lurkdict()
        {
            Rnd = new Random();

            Lurks = new List<Lurk>();

            var fileManager = new FileManager();
            var data = fileManager.Read();

            var lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var items = line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 2 && !string.IsNullOrWhiteSpace(items[0]) && !string.IsNullOrWhiteSpace(items[1]))
                    Lurks.Add(new Lurk(items[0], items[1]));
            }
        }

        public static List<Lurk> Lurks { get; set; }

        public static string Process(string data)
        {
            foreach(var lurk in Lurks)
            {
                data = lurk.Replace(data);
            }

            return data;
        }
    }

    public class Lurk
    {
        public Lurk()
        {
            Meanings = new List<string>();
        }

        public Lurk(string word, string meanings): this()
        {
            Word = word.Trim();
            Meanings = meanings.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public string Word { get; set; }
        public List<string>Meanings { get; set; }

        public string GetMeaning()
        {
            if (Meanings.Count == 1)
                return Meanings[0];

            return Meanings[Lurkdict.Rnd.Next(Meanings.Count)];
        }

        public string Replace(string data)
        {
            return data.Replace(Word, GetMeaning(), StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public static class StringExtensions
    {
        public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
        {
            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }

    }
}