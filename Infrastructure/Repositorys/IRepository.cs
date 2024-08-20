using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys
{
    public interface IRepository<T>
    {
        Task<T>? AddAsync(T entity);
        T Update(T entity);
        Task<T?> GetAsync(Guid id, bool isInclude = false);
        Task<IList<T>> GetAllAsync(bool isInclude = false);
        Task<int> SaveChangesAsync();
        Task<bool> ExistsAsync(Guid id);
    }
}
