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
        bool Save(string id, string value);
        bool Edit(string id, string value);
        bool DeleteObject(string id);
    }
}