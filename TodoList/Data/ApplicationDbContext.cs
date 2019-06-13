using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(key => key.ToDoItemId);
                entity.Property(p => p.Title).HasMaxLength(50).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(150).IsRequired();
                entity.Property(p => p.UpdatedDateTime).IsRequired();
                entity.Property(p => p.IsComplete).HasDefaultValue(0);

                entity.HasOne(p => p.ApplicationUser)
                    .WithMany(b => b.TodoItems)
                    .HasForeignKey(p => p.ApplicationUserId)
                    .HasConstraintName("FK_ToDoItem_User");
            });
        }
    }
}
