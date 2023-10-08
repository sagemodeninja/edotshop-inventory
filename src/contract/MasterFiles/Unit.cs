﻿using EdotShop.Contracts.Inventory.Enums;

namespace EdotShop.Contracts.Inventory;

public class Unit : IUnit
{
    public long Id { get; set; }

    public Guid ObjectId { get; set; }

    public string Code { get; set; }

    public string Symbol { get; set; }

    public string Description { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}