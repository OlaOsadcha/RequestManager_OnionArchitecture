using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Core.Application.Interfaces
{
    public interface ILifecycleRepository : IAsyncRepository<Lifecycle>
    {
    }
}