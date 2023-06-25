using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Core.Application.Interfaces
{
    public interface IRequestRepository : IAsyncRepository<Request>
    {
        public Task<List<Request>> GetAllRequestForUserAsync(int userId);

        public Task<List<Request>> GetAllRequestForModeratorAsync();
        
        public Task<List<Request>> GetAllRequestForExecutorAsync(int userId);

        public Task<bool> IsCategoryExists(int requestId);

        public Task<bool> IsUserExists(int requestId);

        public Task<bool> IsExecutorExists(int requestId);

        public Task<bool> IsSubDepartmentExists(int requestId); 
    }
}