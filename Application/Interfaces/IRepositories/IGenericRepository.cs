using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(long id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}