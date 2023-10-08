using Microsoft.EntityFrameworkCore;

namespace EdotShop.Services.Inventory;

public class ServiceContext : DbContext
{
      public ServiceContext(DbContextOptions<ServiceContext> options)
            : base(options)
      {
      }
      
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
      }
}
