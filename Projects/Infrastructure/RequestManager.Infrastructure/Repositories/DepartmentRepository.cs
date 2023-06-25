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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<bool> IsDepartmentAlreadyExists(string name)
        { 
          return await this._appDbContext.Departments
                .AsNoTracking().AnyAsync(x => x.Name == name);
        }
    }
}