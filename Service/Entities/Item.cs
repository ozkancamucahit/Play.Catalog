using Common.Lib.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Service.Entities;

public sealed class Item : IEntity
{
    [BsonId(IdGenerator = typeof(AscendingGuidGenerator))]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    public decimal Price { get; set; }

    [BsonRepresentation(BsonType.Document)]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
}
