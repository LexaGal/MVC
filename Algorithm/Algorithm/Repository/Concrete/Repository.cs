using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Algorithm.Database;
using Algorithm.DomainModels;
using Algorithm.Repository.Abstract;
using log4net;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;

namespace Algorithm.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T: class
    {
        protected AlgorithmDb Context = new AlgorithmDb();
        protected string ConnectionString = ConfigurationManager.ConnectionStrings["AlgorithmDb"].ConnectionString;
        public IQueryable<T> GetAll()
        {
            MvcApplication.Log.Info("E");
            return Context.Set<T>().AsQueryable();
        }

        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public bool Add(T value)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Context.Set<T>().Add(value);
                Context.SaveChanges();
                Dispose();
                scope.Complete();
                return true;
            }
        }

        public bool Edit(int id, T value)
        {
            return true;
        }

        public bool Delete(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                T t = Context.Set<T>().Find(id);
                if (t != null)
                {
                    Context.Set<T>().Remove(t);
                    Context.SaveChanges();
                    Dispose();
                    scope.Complete();
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}