using EdotShop.Contracts.Inventory.Enums;

namespace EdotShop.Contracts.Inventory.Interfaces;

public interface ITransaction : IBasicEntity
{
    public DateTime Timestamp { get; set; }

    public string Customer { get; set; }

    public decimal Amount { get; set; }

    public decimal Markup { get; set; }

    public decimal Total { get; set; }

    public new TransactionStatus Status { get; set; }
}
