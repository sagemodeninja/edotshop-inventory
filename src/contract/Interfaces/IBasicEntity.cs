using EdotShop.Contracts.Inventory.Enums;

namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IBasicEntity : IBaseEntity
{
    public string Code { get; }

    public string Description { get; }

    public string? Remarks { get; }

    public GenericEntityStatus Status { get; }
}
