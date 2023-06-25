using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Infrastructure.Manager
{
    public class DataManager
    {
        public IUserRepository RequestUserRepository { get; set; }

        public IRequestRepository RequestRepository { get; set; }

        public IDepartmentRepository DepartmentRepository { get; set; }

        public ISubDepartmentRepository SubDepartmentRepository { get; set; }   

        public ICategoryRepository CategoryRepository { get; set; }
        public ILifecycleRepository LifecycleRepository { get; set; }

        public DataManager(IUserRepository requestUserRepository, IRequestRepository requestRepository,
           IDepartmentRepository departmentRepository, ISubDepartmentRepository subDepartmentRepository,
           ICategoryRepository categoryRepository, ILifecycleRepository lifecycleRepository)
        {
            this.RequestUserRepository = requestUserRepository;
            this.RequestRepository = requestRepository;
            this.DepartmentRepository = departmentRepository;
            this.SubDepartmentRepository = subDepartmentRepository;
            this.CategoryRepository = categoryRepository;
            LifecycleRepository = lifecycleRepository;
        }
    }
}