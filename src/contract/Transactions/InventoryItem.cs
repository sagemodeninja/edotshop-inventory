using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EdotShop.Contracts.Inventory.Enums;
using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

[Table("InventoryItems")]
public class InventoryItem : IInventoryItem
{
    public long Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ObjectId { get; set; }

    public Guid InventoryId { get; set; }

    public Inventory? Inventory { get; set; }

    [MaxLength(30)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    public Guid SupplierId { get; set; }

    public Supplier? Supplier { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}
