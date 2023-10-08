using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

public interface ITransactionDetail : IBasicEntity
{
    public Guid InventoryItemId { get; }

    public decimal Amount { get; }

    public int Quantity { get; }

    public decimal Total { get; }
}
