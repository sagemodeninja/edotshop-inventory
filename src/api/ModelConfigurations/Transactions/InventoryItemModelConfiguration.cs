using Asp.Versioning;
using Asp.Versioning.OData;
using EdotShop.Contracts.Inventory;
using Microsoft.OData.ModelBuilder;

namespace EdotShop.InventoryServices.ModelConfigurations;

/// <summary>
/// OData model configuration for inventory item entities.
/// </summary>
public class InventoryItemModelConfiguration : IModelConfiguration
{
    /// <inheritdoc/>
    public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
    {
        var entity = builder.EntitySet<InventoryItem>("InventoryItems").EntityType;

        entity.Ignore(dtr => dtr.Id);
        entity.HasKey(dtr => dtr.ObjectId);
    }
}
