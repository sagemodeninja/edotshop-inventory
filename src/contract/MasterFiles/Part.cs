using EdotShop.Contracts.Inventory.Enums;
using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

public class Part : IPart
{
    public long Id { get; set; }

    public Guid ObjectId { get; set; }

    public Guid ClassificationId { get; set; }

    public Classification? Classification { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public Guid UnitId { get; set; }

    public Unit? Unit { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}
