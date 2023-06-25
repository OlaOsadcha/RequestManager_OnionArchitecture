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
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<bool> IsCategoryAlreadyExists(string name)
        {
            return await this._appDbContext.Categories
                  .AsNoTracking().AnyAsync(x => x.Name == name);
        }
    }
}