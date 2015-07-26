using System.Linq;
using Algorithm.Database;
using Algorithm.Repository.Abstract;

namespace Algorithm.Repository.Concrete
{
    public abstract class Repository<T> : IRepository<T> where T: class 
    {
        private AlgorithmDb _context = new AlgorithmDb();
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public bool Add(T value)
        {
            _context.Set<T>().Add(value);
            _context.SaveChanges();
            return true;
        }

        public bool Edit(T value)
        {
            T result = _context.Set<T>().First(v => v == value);
            if (result != null)
            {
                result = value;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            _context.Set<T>().Remove(_context.Set<T>().Find(id));
            _context.SaveChanges();
            return true;
        }

        public void Dispose()
        {
        }

    }
}