using System.Configuration;
using System.Linq;
using Aop.AopAspects;
using Aop.AopAspects.Logging;
using DatabaseAccess.DatabaseContext;
using DatabaseAccess.Repository.Abstract;

namespace DatabaseAccess.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T: class
    {
        protected AlgorithmDb Context = new AlgorithmDb();
        protected string ConnectionString = ConfigurationManager.ConnectionStrings["AlgorithmDb"].ConnectionString;

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>().AsQueryable();
        }

        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        [AuthentificationAspect]
        [LogAspect] 
        [RunInTransactionAspect]
        public bool Add(T value)
        {
            Context.Set<T>().Add(value);
            Context.SaveChanges();
            Dispose();
            return true;
        }

        public bool Edit(int id, T value)
        {       
            return true;
        }

        [AuthentificationAspect]
        [LogAspect] 
        [RunInTransactionAspect]
        public bool Delete(int id)
        {
            T t = Context.Set<T>().Find(id);
            if (t != null)
            {
                Context.Set<T>().Remove(t);
                Context.SaveChanges();
                Dispose();
                return true;
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