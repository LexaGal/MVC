using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Algorithm.Repository
{
    public class FileRepository : IRepository
    {
        private StreamReader Reader { get; set; }
        private const string Extension = ".txt";
        private const string Path = "B:/";
        private Dictionary<string, string> FilesDictionary { get; set; }

        public FileRepository()
        {
            Reader = new StreamReader(string.Format("{0}", ConfigurationManager.AppSettings["Names"]));
            IEnumerable<string> names = Regex.Split(Reader.ReadToEnd(), "\r\n");
            FilesDictionary = new Dictionary<string, string>();
            foreach (var name in names)
            {
                StreamReader reader = new StreamReader(string.Format("{0}{1}{2}", Path, name, Extension));
                FilesDictionary.Add(name, reader.ReadToEnd());
                reader.Close();
            }
        }

        public IEnumerable<string> GetAll(string type)
        {
            if (type == "Names")
            {
                return FilesDictionary.Keys;
                
            }
            if (type == "Values")
            {
                return FilesDictionary.Values;
            }
            return FilesDictionary.Keys;
        }

        public string Get(string id)
        {
            return FilesDictionary[id];
        }

        public void Save(string id)
        {}

        public void Edit(string id)
        {}

        public void DeleteObject(string id)
        {}
    }
}