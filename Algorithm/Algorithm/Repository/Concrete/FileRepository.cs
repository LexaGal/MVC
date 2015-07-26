//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using WebGrease.Css.Extensions;

//namespace Algorithm.Repository
//{
//    public class FileRepository : IRepository
//    {
//        private StreamReader Reader { get; set; }
//        private IDictionary<string, string> FilesDictionary { get; set; }

//        public FileRepository()
//        {
//            Reader = new StreamReader(string.Format("{0}", ConfigurationManager.AppSettings["Names"]));
//            IEnumerable<string> names = Regex.Split(Reader.ReadToEnd(), "\r\n");
//            Reader.Close();
//            FilesDictionary = new Dictionary<string, string>();
//            foreach (var name in names)
//            {
//                StreamReader reader = new StreamReader(name);
//                FilesDictionary.Add(name, reader.ReadToEnd());
//                reader.Close();
//            }
//        }

//        public IDictionary<string, string> GetAll()
//        {
//            return FilesDictionary;
//        }

//        public string Get(string id)
//        {
//            try
//            {
//                return FilesDictionary[id];
//            }
//            catch (KeyNotFoundException)
//            {
//                return null;
//            }
//        }

//        public bool Save(string id, string value)
//        {
//            if (!FilesDictionary.ContainsKey(id))
//            {
//                FilesDictionary.Add(id, value);
//                return true;
//            }
//            return false;
//        }

//        public bool Edit(string id, string value)
//        {
//            if (FilesDictionary.ContainsKey(id))
//            {
//                FilesDictionary.Remove(id);
//                FilesDictionary.Add(id, value);
//                return true;
//            }
//            return false;
//        }

//        public bool Delete(string id)
//        {
//            if (FilesDictionary.ContainsKey(id))
//            {
//                FilesDictionary.Remove(id);
//                return true;
//            } 
//            return false;
//        }
//    }
//}