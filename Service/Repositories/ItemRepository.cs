using MongoDB.Driver;
using Service.Entities;

namespace Service.Repositories;

public sealed class ItemRepository : IItemRepository
{
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> dbcollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public ItemRepository(IMongoDatabase database)
    {
        dbcollection = database.GetCollection<Item>(collectionName);
    }


    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await dbcollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbcollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Item entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbcollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Item entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await dbcollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbcollection.DeleteOneAsync(filter);
    }

}
