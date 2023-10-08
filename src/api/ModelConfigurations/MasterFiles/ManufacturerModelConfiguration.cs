using Asp.Versioning;
using Asp.Versioning.OData;
using EdotShop.Contracts.Inventory;
using Microsoft.OData.ModelBuilder;

namespace EdotShop.InventoryServices.ModelConfigurations;

/// <summary>
/// OData model configuration for manufacturer entities.
/// </summary>
public class ManufacturerModelConfiguration : IModelConfiguration
{
    /// <inheritdoc/>
    public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
    {
        var entity = builder.EntitySet<Manufacturer>("Manufacturers").EntityType;

        entity.Ignore(dtr => dtr.Id);
        entity.HasKey(dtr => dtr.ObjectId);
    }
}
