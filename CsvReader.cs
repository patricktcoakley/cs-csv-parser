using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvParser
{
    public class CsvReader
    {
        private readonly string[] _columns;
        private readonly StreamReader _csvFile;

        public CsvReader(string filePath)
        {
            try
            {
                var file = new FileStream(filePath, FileMode.Open);
                _csvFile = new StreamReader(file);
                _columns = _csvFile.ReadLine().Split(',');
            }
            catch (Exception e) when (e is NullReferenceException || e is FileNotFoundException)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public IEnumerable<IEnumerable<KeyValuePair<string, string>>> Lines
        {
            get
            {
                string line;
                var entries = new List<IEnumerable<KeyValuePair<string, string>>>();

                if (_csvFile == null) return entries;

                while ((line = _csvFile.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    var entry =
                        Enumerable.Zip(_columns, values,
                            (key, val) => new KeyValuePair<string, string>(key, val));

                    entries.Add(entry);
                }

                return entries;
            }
        }

        public IEnumerable<string> GetValuesByKey(string key)
        {
            return Lines
                .Select(entry => entry.First(pair => pair.Key == key).Value)
                .ToList();
        }

        public string GetKeyByValue(string value)
        {
            return Lines
                .Select(entry => entry.First(pair => pair.Value.Contains(value)).Key)
                .ToString();
        }
    }
}