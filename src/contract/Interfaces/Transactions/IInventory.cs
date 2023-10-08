namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IInventory : IBasicEntity
{
    public Guid PartId { get; }

    public string PartNumber { get; }

    public Guid ManufacturerId { get; }

    public string Model { get; }
    
    public bool IsOriginal { get; }
}
