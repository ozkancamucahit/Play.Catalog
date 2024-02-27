using Common.Lib.Entities;
using Common.Lib.MongoDB;
using Common.Lib.Repositories;
using MongoDB.Driver;
using Service.DTOs;
using Service.Entities;
namespace Service.Helpers.Extensions;

public static class Extensions
{
    public static ItemDTO AsDto(this Item item)
    {
        return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
    {
        services
            .AddSingleton<IRepository<Item>>(serviceProvidewr =>
            {
                var dataBase = serviceProvidewr.GetService<IMongoDatabase>() ?? throw new ArgumentNullException(nameof(IMongoDatabase));
                return new MongoRepository<Item>(dataBase, collectionName);
            });
        return services;
    }

}
