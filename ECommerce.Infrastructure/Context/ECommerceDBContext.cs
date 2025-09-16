using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Context
{
    public class ECommerceDBContext(DbContextOptions<ECommerceDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User - Role (many-to-many)
            modelBuilder.Entity<User>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("RoleUsers"));

            // Permission - Role (many-to-many)
            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Permissions)
                .UsingEntity(j => j.ToTable("RolePermissions"));


            // Category Entity Configurations
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Product Entity Configurations
            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Stock)
                .IsRequired();

            // Relationship: One-to-Many between Category and Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
