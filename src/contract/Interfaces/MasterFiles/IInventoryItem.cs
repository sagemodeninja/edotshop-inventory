namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IInventoryItem : IBasicEntity
{
    public Guid InventoryId { get; set; }

    public Guid SupplierId { get; set; }

    public decimal Price { get; set; }
}
