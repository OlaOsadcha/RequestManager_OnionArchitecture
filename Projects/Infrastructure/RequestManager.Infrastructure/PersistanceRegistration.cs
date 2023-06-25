using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Email;
using RequestManager.Infrastructure.Configurations.Models;
using RequestManager.Infrastructure.Email;
using RequestManager.Infrastructure.Log;
using RequestManager.Infrastructure.Manager;
using RequestManager.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = RequestManager.Core.Application.Log.ILogger;

namespace RequestManager.Infrastructure
{
    public static class PersistanceRegistration
    {
        public static IServiceCollection AddPersistanceRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x =>
            {   
                x.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
                x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();  
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<ISubDepartmentRepository, SubDepartmentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILifecycleRepository, LifecycleRepository>();

            services.AddScoped<DataManager>();
            //services
            services.AddScoped<ILogger, NLogger>();
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}