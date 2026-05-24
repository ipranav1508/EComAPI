using EComAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EComAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Image> Images { get; set; }

    }
}
