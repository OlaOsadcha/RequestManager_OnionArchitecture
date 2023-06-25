using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Core.Application.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        public Task<T?> GetByIdAsync(int id);
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
    }
}