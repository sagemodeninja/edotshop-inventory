using EdotShop.Contracts.Inventory.Interfaces;

namespace EdotShop.Contracts.Inventory;

public interface IUnit : IBasicEntity
{
    public string Symbol { get; }
}
