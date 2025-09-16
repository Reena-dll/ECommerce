using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using (var context = new ECommerceDbContext())
            {
                // Apply pending migrations to database
                await context.Database.MigrateAsync();
                Console.WriteLine("Database migration completed.");
            }

            Console.WriteLine("E-commerce microservice running...");
        }
    }

    // DbContext representing the E-commerce database
    public class ECommerceDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database~=ECommerceDb;Trusted_Connection=false;");
        }
    }

    // Category entity representing categories of products
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }

    // Product entity representing products in a category
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    // ProductRepository class
    public class ProductRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }

    public class ProductController
    {
        private readonly ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        public void CreateProduct(Product product)
        {
            _repository.AddProduct(product);
            Console.WriteLine($"Product '{product.Name}' added successfully.");
        }

        public void ListAllProducts()
        {
            var products = _repository.GetAllProducts();
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.Name}, Price: {product.Price}, Stock: {product.Stock}");
            }
        }
    }
}

public class Startup
{
    public async Task Run()
    {
        var context = new ECommerceService.ECommerceDbContext();
        var repository = new ECommerceService.ProductRepository(context);
        var controller = new ECommerceService.ProductController(repository);

        controller.CreateProduct(new ECommerceService.Product { Name = "Laptop", Price = 1000M, Stock = 10 });
        controller.ListAllProducts();
    }
}

