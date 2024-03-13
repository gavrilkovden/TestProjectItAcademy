using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;

namespace Common.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.UserName).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.OwnerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
