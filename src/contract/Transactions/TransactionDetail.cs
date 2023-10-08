using EdotShop.Contracts.Inventory.Enums;

namespace EdotShop.Contracts.Inventory;

public class TransactionDetail : ITransactionDetail
{
    public long Id { get; set; }

    public Guid ObjectId { get; set; }

    public Guid InventoryItemId { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}
