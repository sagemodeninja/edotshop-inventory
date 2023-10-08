﻿using EdotShop.Contracts.Inventory.Enums;

namespace EdotShop.Contracts.Inventory.Interfaces;

public interface ITransaction : IBaseEntity
{
    public string Code { get; }

    public string Description { get; }

    public DateTime Timestamp { get; }

    public string Customer { get; }

    public decimal Amount { get; }

    public decimal Markup { get; }

    public decimal Total { get; }

    public string? Remarks { get; }

    public TransactionStatus Status { get; }
}
