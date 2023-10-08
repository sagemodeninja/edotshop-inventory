using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EdotShop.Contracts.Inventory.Enums;
using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

[Table("Parts")]
public class Part : IPart
{
    public long Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ObjectId { get; set; }

    public Guid ClassificationId { get; set; }

    public Classification? Classification { get; set; }

    [MaxLength(30)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    public Guid UnitId { get; set; }

    public Unit? Unit { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}
