using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Core.Application.Interfaces
{
    public interface IUserRepository : IAsyncRepository<RequestUser>
    {
        public Task<RequestUser?> FindByNameAsync(string name);

        public Task<bool> IsUserAlreadyExists(string name);
    }
}