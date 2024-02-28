

using Common.Lib.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Play.Inventory.Service.Entities;


public sealed class CatalogItem : IEntity
{
    [BsonId(IdGenerator = typeof(AscendingGuidGenerator))]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;

}



