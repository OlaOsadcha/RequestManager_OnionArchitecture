using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Request = RequestManager.Core.Domain.Entities.Request;

namespace RequestManager.Infrastructure.Repositories
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<List<Request>> GetAllRequestForUserAsync(int userId)
        { 
           return await this._appDbContext.Requests.Where(r => r.UserId == userId) 
                            .Include(r => r.Category) 
                            .Include(r => r.Lifecycle)  
                            .Include(r => r.User)         
                            .OrderByDescending(r => r.Lifecycle.Opened).AsNoTracking().ToListAsync();
        }

        public async Task<List<Request>> GetAllRequestForModeratorAsync()
        {
            return await this._appDbContext.Requests.Include(r => r.User)
                            .Include(r => r.Lifecycle)
                            .Include(r => r.Executor)
                            .Where(r => r.ExecutorId == null)
                            .Where(r => r.Status != (int)RequestStatus.Closed).AsNoTracking().ToListAsync();
        }

        public async Task<List<Request>> GetAllRequestForExecutorAsync(int userId)
        {
            return await this._appDbContext.Requests.Include(r => r.User)
                                .Include(r => r.Lifecycle)
                                .Include(r => r.Executor)
                                .Where(r => r.ExecutorId == userId)
                                .Where(r => r.Status != (int)RequestStatus.Closed).AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsCategoryExists(int requestId)
        {
            return await this._appDbContext.Requests
                   .AsNoTracking().Where(x => x.Id == requestId).AnyAsync(x => x.CategoryId.HasValue == true);
        }

        public async Task<bool> IsUserExists(int requestId)
        {
            return await this._appDbContext.Requests
                   .AsNoTracking().Where(x => x.Id == requestId).AnyAsync(x => x.UserId.HasValue == true);
        }

        public async Task<bool> IsExecutorExists(int requestId)
        {
            return await this._appDbContext.Requests
                   .AsNoTracking().Where(x => x.Id == requestId).AnyAsync(x => x.ExecutorId.HasValue == true);
        }

        public async Task<bool> IsSubDepartmentExists(int requestId)
        {
            return await this._appDbContext.Requests
                   .AsNoTracking().Where(x => x.Id == requestId).AnyAsync(x => x.SubDepartmentId.HasValue == true);
        }
    }
}