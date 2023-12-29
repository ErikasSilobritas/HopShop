using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Item>items { get; set; }
        public DbSet<Shop>shops { get; set; }
        public DbSet<PurchaseHistory>purchases { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
