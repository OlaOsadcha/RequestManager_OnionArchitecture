using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RequestManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Infrastructure
{
    public class AppDbContext : IdentityDbContext<RequestUser, RequestRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RequestUser> RequestUsers { get; set; }

        public DbSet<RequestRole> RequestRoles { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Lifecycle> Lifecycles { get; set; }

        public DbSet<SubDepartment> SubDepartments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Uzupełnić do migracji
            optionsBuilder.UseSqlServer(@"Data Source=NB-PROG2; Database = RequestDB2; Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RequestRole>().HasData(new RequestRole()
            {
                Id = 1,
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<RequestUser>().HasData(new RequestUser()
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "test@gmail.com",
                NormalizedEmail = "TEST@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<RequestUser>().HashPassword(null, "zaq1@WSX"),
                SecurityStamp = string.Empty,
                Position = "Programmer",
                DepartmentId = null                
            });

            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>()
            {
                RoleId = 1,
                UserId = 1
            });

            builder.Entity<RequestRole>().HasData(new RequestRole()
            {
                Id = 2,
                Name = "moderator",
                NormalizedName = "MODERATOR"
            });

            builder.Entity<RequestRole>().HasData(new RequestRole()
            {
                Id = 3,
                Name = "user",
                NormalizedName = "USER"
            });

            builder.Entity<RequestRole>().HasData(new RequestRole()
            {
                Id = 4,
                Name = "executor",
                NormalizedName = "EXECUTOR"
            });
        }
    }
}