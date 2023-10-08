namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IPart : IBasicEntity
{
    public Guid ClassificationId { get; }

    public Guid UnitId { get; }
}
