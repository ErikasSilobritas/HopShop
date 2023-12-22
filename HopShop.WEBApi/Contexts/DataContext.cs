using HopShop.WEBApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HopShop.WEBApi.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Item>items { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
