using Common.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;
using Users.Service.Utils;

namespace Common.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationUserApplicationRole> ApplicationUserApplicationRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().HasKey(t => t.Id);
            modelBuilder.Entity<Todo>().Property(t => t.Label).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Todo>().Property(t => t.CreatedDate).IsRequired().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Todo>().Property(t => t.UpdatedDate).IsRequired().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Todo>().Property(t => t.IsDone);
            modelBuilder.Entity<Todo>().Property(t => t.OwnerId).IsRequired();

            modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.UserName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Login).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<ApplicationUser>().Navigation(e => e.Roles).AutoInclude();

            modelBuilder.Entity<RefreshToken>().HasKey(u => u.Id);
            modelBuilder.Entity<RefreshToken>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RefreshToken>().HasOne(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<ApplicationUserApplicationRole>().HasKey(c => new
            {
                c.ApplicationUserId,
                c.ApplicationUserRoleId
            });

            modelBuilder.Entity<ApplicationUserApplicationRole>().Navigation(e => e.ApplicationUserRole).AutoInclude();

            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.Roles)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<ApplicationUserRole>().HasMany(e => e.Users)
    .WithOne(e => e.ApplicationUserRole)
    .HasForeignKey(e => e.ApplicationUserRoleId);

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.OwnerId);

            modelBuilder.Entity<ApplicationUserRole>().HasKey(u => u.Id);
            modelBuilder.Entity<ApplicationUserRole>().Property(u => u.Name).HasMaxLength(50).IsRequired();

            base.OnModelCreating(modelBuilder);

            var adminRoleId = 1;
            var adminUser = new ApplicationUser
            {
                Id = 1,
                UserName = "admin",
                Login = "admin",
                PasswordHash = PasswordHasher.HashPassword("12345678")
            };

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            var adminRole = new ApplicationUserRole
            {
                Id = 1,
                Name = "Admin"
            };

            modelBuilder.Entity<ApplicationUserRole>().HasData(adminRole);

            var userRole = new ApplicationUserRole
            {
                Id = 2,
                Name = "User"
            };

            modelBuilder.Entity<ApplicationUserRole>().HasData(userRole);

        }
    }
}
