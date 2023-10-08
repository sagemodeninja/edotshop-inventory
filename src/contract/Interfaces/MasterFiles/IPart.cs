namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IPart : IBasicEntity
{
    public Guid ClassificationId { get; set; }

    public Guid UnitId { get; set; }
}
