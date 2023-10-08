using EdotShop.Contracts.Inventory.Enums;
using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

public class Transaction : ITransaction
{
    public long Id { get; set; }

    public Guid ObjectId { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public DateTime Timestamp { get; set; }

    public string Customer { get; set; }

    public decimal Amount { get; set; }

    public decimal Markup { get; set; }

    public decimal Total { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string? Remarks { get; set; }

    public TransactionStatus Status { get; set; }
}
