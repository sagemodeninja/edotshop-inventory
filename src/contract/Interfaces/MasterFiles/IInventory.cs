namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IInventory : IBasicEntity
{
    public Guid PartId { get; set; }

    public Guid ManufacturerId { get; set; }

    public string PartNumber { get; set; }

    public string Model { get; set; }
    
    public bool IsOriginal { get; set; }
}
