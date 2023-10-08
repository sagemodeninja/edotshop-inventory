using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

public interface ITransactionDetail : IBasicEntity
{
    public Guid InventoryItemId { get; set; }

    public decimal Amount { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }
}
