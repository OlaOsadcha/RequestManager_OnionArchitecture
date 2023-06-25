using Microsoft.EntityFrameworkCore;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Infrastructure.Repositories
{
    public class SubDepartmentRepository : BaseRepository<SubDepartment>, ISubDepartmentRepository
    {
        public SubDepartmentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async new Task<IReadOnlyList<SubDepartment>> GetAllAsync()
        {
            return await this._appDbContext.Set<SubDepartment>().Include(x => x.Department)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsSubDepartmentAlreadyExists(string name)
        {
            return await this._appDbContext.
                SubDepartments.AnyAsync(x => x.CabNumber == name);
        }

        public async Task<bool> IsDepartmentExists(int id)
        {
            return await this._appDbContext.
                    SubDepartments.Include(x => x.Department).AnyAsync(x => x.Id == id && x.DepartmentId.HasValue);
        }
    }
}