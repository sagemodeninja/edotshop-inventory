using EdotShop.Contracts.Inventory.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdotShop.InventoryServices.Extensions;

/// <summary>
/// Provides extensions for configuring entities.
/// </summary>
public static class EntityTypeBuilderExtensions
{
      /// <summary>
      /// Apply default configuration for entities.
      /// </summary>
      /// <param name="entity">The entity to configure.</param>
      public static void ConfigureBaseEntity(this EntityTypeBuilder entity) {
            entity.Property(nameof(IBaseEntity.CreatedOn))
                  .HasDefaultValueSql("GETDATE()");
            
            entity.Property(nameof(IBaseEntity.UpdatedOn))
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAddOrUpdate();

            entity.HasAlternateKey(nameof(IBaseEntity.ObjectId));
      }
}
