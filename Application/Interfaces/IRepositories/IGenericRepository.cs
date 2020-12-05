using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task<bool> Exist(int id);
    }
}