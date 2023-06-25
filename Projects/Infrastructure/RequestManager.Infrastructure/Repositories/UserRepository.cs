using Microsoft.EntityFrameworkCore;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<RequestUser>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

       public new async Task<IReadOnlyList<RequestUser>> GetAllAsync()
        {
            return await _appDbContext.Set<RequestUser>().AsNoTracking()
                .Include(x => x.Department)
                .ToListAsync();
        }

        public async Task<RequestUser?> FindByNameAsync(string name)
        {
            return await this._appDbContext.RequestUsers
                .AsNoTracking().FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<bool> IsUserAlreadyExists(string name)
        {
            return await this._appDbContext.RequestUsers
                .AsNoTracking().AnyAsync(x => x.UserName == name);
        }
    }
}