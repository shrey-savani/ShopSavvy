using Microsoft.EntityFrameworkCore;
using System.Net;
using ShopSavvy.Models;

namespace ShopSavvy.Models
{
    public class ToyStoreDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ToyStoreDBContext(DbContextOptions<ToyStoreDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Toys" },
                new Category { Id = 2, Name = "Games" },
                new Category { Id = 3, Name = "Educational Toys" },
                new Category { Id = 4, Name = "Dolls and Action Figures" },
                new Category { Id = 5, Name = "Building Blocks and Construction Sets" }
            );

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "admin", Role = "Admin" },
                new User { Id = 2, Username= "JohnDoe", Password = "password123", Role="User"},
                new User { Id = 3, Username= "JaneSmith", Password = "password456", Role = "User" }
            );

            // Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Toy Car", Description = "Remote-controlled toy car",ImagePath="toy_1.jpg", Price = 29.99m, CategoryId = 1 },
                new Product { Id = 2, Name = "Board Game", Description = "Family board game",ImagePath="toy_2.jpg", Price = 19.99m, CategoryId = 2 }
            );

            // Orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    OrderItems= "[Toy Car(5),Board Game(10)]",
                    ShippingAddress = "123 Main St, Cityville",
                    City = "Cityville",
                    State = "Stateville",
                    ZipCode = "12345",
                    PhoneNumber = "123-456-7890"
                }
            );
        }

        public DbSet<ShopSavvy.Models.Category>? Category { get; set; }
    }
}
