using Asp.Versioning;
using Asp.Versioning.OData;
using EdotShop.Contracts.Inventory;
using Microsoft.OData.ModelBuilder;

namespace EdotShop.InventoryServices.ModelConfigurations;

/// <summary>
/// OData model configuration for sale item entities.
/// </summary>
public class SaleItemModelConfiguration : IModelConfiguration
{
    /// <inheritdoc/>
    public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
    {
        var entity = builder.EntitySet<SaleItem>("SaleItems").EntityType;

        entity.Ignore(dtr => dtr.Id);
        entity.HasKey(dtr => dtr.ObjectId);
    }
}
