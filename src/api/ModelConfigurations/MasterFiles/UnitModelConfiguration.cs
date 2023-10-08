using Asp.Versioning;
using Asp.Versioning.OData;
using EdotShop.Contracts.Inventory;
using Microsoft.OData.ModelBuilder;

namespace EdotShop.InventoryServices.ModelConfigurations;

/// <summary>
/// OData model configuration for unit entities.
/// </summary>
public class UnitModelConfiguration : IModelConfiguration
{
    /// <inheritdoc/>
    public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
    {
        var entity = builder.EntitySet<Unit>("Units").EntityType;

        entity.Ignore(dtr => dtr.Id);
        entity.HasKey(dtr => dtr.ObjectId);
    }
}
