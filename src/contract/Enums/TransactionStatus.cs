using System.Text.Json.Serialization;

namespace EdotShop.Contracts.Inventory.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionStatus
{

}
