using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EdotShop.Contracts.Inventory.Enums;

namespace EdotShop.Contracts.Inventory;

[Table("SaleItems")]
public class SaleItem : ISaleItem
{
    public long Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ObjectId { get; set; }

    public Guid SaleId { get; set; }

    public Sale? Sale { get; set; }

    public Guid InventoryItemId { get; set; }

    public InventoryItem? InventoryItem { get; set; }

    [MaxLength(30)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    public GenericEntityStatus Status { get; set; }
}
