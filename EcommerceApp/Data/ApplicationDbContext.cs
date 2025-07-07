using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace EcommerceApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Corrected the issue by using the 'modelBuilder' parameter instead of 'ModelBuilder'
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address1)
                .WithMany()
                .HasForeignKey(o => o.AddressId) // Corrected foreign key property
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "Processing" },
                new Status { Id = 3, Name = "Shipped" },
                new Status { Id = 4, Name = "Delivered" },
                new Status { Id = 5, Name = "Canceled" },
                new Status { Id = 6, Name = "Completed" },
                new Status { Id = 7, Name = "Failed" },
                new Status { Id = 8, Name = "Approved" },
                new Status { Id = 9, Name = "Rejected" },
                new Status { Id = 10, Name = "Refunded" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories" },
                new Category { Id = 2, Name = "Books", Description = "Books and magazines" },
                new Category { Id = 3, Name = "Clothes", Description = "Shirts and Jeans" },
                new Category { Id = 4, Name = "Foods", Description = "Vegetables and fastfoods" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Smartphone",
                    Description = "Latest model smartphone with advanced features.",
                    Price = 699.99m,
                    StockQuantity = 50,
                    ImageUrl = "https://example.com/images/smartphone.jpg",
                    DiscountPercent = 10,
                    CategoryId = 1,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop",
                    Description = "High-performance laptop suitable for all your needs.",
                    Price = 999.99m,
                    StockQuantity = 30,
                    ImageUrl = "https://example.com/images/laptop.jpg",
                    DiscountPercent = 15,
                    CategoryId = 1,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Science Fiction Novel",
                    Description = "A thrilling science fiction novel set in the future.",
                    Price = 19.99m,
                    StockQuantity = 100,
                    ImageUrl = "https://example.com/images/scifi-novel.jpg",
                    DiscountPercent = 5,
                    CategoryId = 2,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 4,
                    Name = "Horror Novel",
                    Description = "This is about horror scene about a group of Students visiting an abandoned hospital.",
                    Price = 35.53m,
                    StockQuantity = 20,
                    ImageUrl = "https://example.com/images/horror-novel.jpg",
                    DiscountPercent = 10,
                    CategoryId = 4,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 5,
                    Name = "T-shirt",
                    Description = "This is a white T-shirt and is suitable for one who love peace.",
                    Price = 150.53m,
                    StockQuantity = 35,
                    ImageUrl = "https://example.com/images/white-Tshirt.jpg",
                    DiscountPercent = 10,
                    CategoryId = 3,
                    IsAvailable = true
                }
            );
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=THANHLE;Database=ECommerceDB;Trusted_Connection=True;");
        //}
    }
}
