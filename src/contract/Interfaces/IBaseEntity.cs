namespace EdotShop.Contracts.Inventory.Interfaces;

public interface IBaseEntity
{
    public long Id { get; }

    public Guid ObjectId { get; }

    public DateTime CreatedOn { get; }

    public DateTime UpdatedOn { get; }
}
