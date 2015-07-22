using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algorithm.Repository
{
    public interface IRepository
    {
        IEnumerable<string> GetAll(string type);
        string Get(string id);
        void Save(string id);
        void Edit(string id);
        void DeleteObject(string id);
    }
}