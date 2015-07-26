using System;
using System.Linq;

namespace Algorithm.Repository.Abstract
{
    public interface IRepository<T>: IDisposable
    {
        IQueryable<T> GetAll();
        T Get(int id);
        bool Add(T value);
        bool Edit(T value);
        bool Delete(int id);
    }
}