using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

public interface ISaleItem : IBasicEntity
{
    public Guid SaleId { get; }

    public Guid InventoryItemId { get; }

    public decimal Amount { get; }

    public int Quantity { get; }

    public decimal Total { get; }
}
