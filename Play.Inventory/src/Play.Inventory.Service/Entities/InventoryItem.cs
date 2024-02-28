

using Common.Lib.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Play.Inventory.Service.Entities;


public sealed class InventoryItem : IEntity
{
    [BsonId(IdGenerator = typeof(AscendingGuidGenerator))]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }


    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonRepresentation(BsonType.String)]

    public Guid CatalogItemId { get; set; }

    public int Quantity { get; set; }

    [BsonRepresentation(BsonType.Document)]

    public DateTimeOffset AcquiredData { get; set; }

}



