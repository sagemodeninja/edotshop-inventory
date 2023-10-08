namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IInventoryItem : IBasicEntity
{
    public Guid InventoryId { get; }

    public Guid SupplierId { get; }

    public decimal Price { get; }
}
