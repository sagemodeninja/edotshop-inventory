using EdotShop.Contracts.Inventory;
using EdotShop.InventoryServices.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EdotShop.InventoryServices;

public class ServiceContext : DbContext
{
      public ServiceContext(DbContextOptions<ServiceContext> options)
            : base(options)
      {
      }

      // Master Files
      public virtual DbSet<Classification> Classifications { get; set; }

      public virtual DbSet<Unit> Units { get; set; }

      public virtual DbSet<Part> Parts { get; set; }

      public virtual DbSet<Manufacturer> Manufacturers { get; set; }

      public virtual DbSet<Supplier> Suppliers { get; set; }

      // Transactions
      public virtual DbSet<Inventory> Inventories { get; set; }

      public virtual DbSet<InventoryItem> InventoryItems { get; set; }

      public virtual DbSet<Sale> Sales { get; set; }

      public virtual DbSet<SaleItem> SaleItems { get; set; }

      
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            // Master Files
            modelBuilder.Entity<Classification>()
                        .ConfigureBaseEntity();

            modelBuilder.Entity<Unit>()
                        .ConfigureBaseEntity();

            modelBuilder.Entity<Part>(entity => {
                  entity.HasOne(part => part.Classification)
                        .WithMany()
                        .HasPrincipalKey(c => c.ObjectId);
                  
                  entity.HasOne(part => part.Unit)
                        .WithMany()
                        .HasPrincipalKey(u => u.ObjectId);
                        
                  entity.ConfigureBaseEntity();
            });

            modelBuilder.Entity<Manufacturer>()
                        .ConfigureBaseEntity();

            modelBuilder.Entity<Supplier>()
                        .ConfigureBaseEntity();

            // Transactions
            modelBuilder.Entity<Inventory>(entity => {
                  entity.HasMany(inventory => inventory.InventoryItems)
                        .WithOne(item => item.Inventory)
                        .HasPrincipalKey(i => i.ObjectId);

                  entity.HasOne(part => part.Part)
                        .WithMany()
                        .HasPrincipalKey(p => p.ObjectId);
                  
                  entity.HasOne(part => part.Manufacturer)
                        .WithMany()
                        .HasPrincipalKey(m => m.ObjectId);
                        
                  entity.ConfigureBaseEntity();
            });

            modelBuilder.Entity<InventoryItem>(entity => {
                  entity.HasOne(item => item.Supplier)
                        .WithMany()
                        .HasPrincipalKey(s => s.ObjectId);
                        
                  entity.ConfigureBaseEntity();
            });

            modelBuilder.Entity<Sale>(entity => {
                  entity.HasMany(sale => sale.SaleItems)
                        .WithOne(item => item.Sale)
                        .HasPrincipalKey(i => i.ObjectId);
                        
                  entity.ConfigureBaseEntity();
            });

            modelBuilder.Entity<SaleItem>(entity => {
                  entity.HasOne(item => item.InventoryItem)
                        .WithMany()
                        .HasPrincipalKey(i => i.ObjectId);
                        
                  entity.ConfigureBaseEntity();
            });
      }
}
