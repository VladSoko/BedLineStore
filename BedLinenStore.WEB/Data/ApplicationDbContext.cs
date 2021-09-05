using BedLinenStore.WEB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BedLinenStore.WEB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MainInfo> MainInfos { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FreelanceSewing> FreelanceSewings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<FreelanceSewing>().ToTable("FreelanceSewing");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<MainInfo>().ToTable("MainInfo");
            modelBuilder.Entity<CartLine>().ToTable("CartLine");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Order>().ToTable("Order");
        }
    }
}