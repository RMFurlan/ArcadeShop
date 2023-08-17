using meusite.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace meusite.Data
{
    public class MeuSiteContext : DbContext
    {
        public MeuSiteContext()
        {
        }
        public MeuSiteContext(DbContextOptions<MeuSiteContext> options) : base(options) 
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "server=localhost;User Id=developer;Password=Rafael804637;database=meusiteappdb";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
