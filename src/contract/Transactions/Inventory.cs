using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EdotShop.Contracts.Inventory.Enums;
using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

[Table("Inventories")]
public class Inventory : IInventory
{
    public long Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ObjectId { get; set; }

    [MaxLength(30)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    [MaxLength(200)]
    public string Model { get; set; }

    public Guid PartId { get; set; }

    public Part? Part { get; set; }

    [MaxLength(120)]
    public string PartNumber { get; set; }

    public Guid ManufacturerId { get; set; }

    public Manufacturer? Manufacturer { get; set; }

    public bool IsOriginal { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public List<InventoryItem>? InventoryItems { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}
