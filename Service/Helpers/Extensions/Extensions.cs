using MongoDB.Driver;
using Service.DTOs;
using Service.Entities;
using Service.Repositories;
using Service.Settings;

namespace Service.Helpers.Extensions;

public static class Extensions
{
    public static ItemDTO AsDto(this Item item)
    {
        return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }

    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services
            .AddSingleton(serviceProvider =>
             {
                 var config = serviceProvider.GetService<IConfiguration>() ?? throw new ArgumentNullException(nameof(IConfiguration));
                 var serviceSettings = config
                    .GetSection(nameof(ServiceSettings))
                    .Get<ServiceSettings>() ?? throw new ArgumentNullException(nameof(ServiceSettings));

                 var mongoDBSettings = config
                     .GetSection(nameof(MongoDBSettings))
                     .Get<MongoDBSettings>() ?? throw new ArgumentNullException(nameof(MongoDBSettings));

                 var mongoClient = new MongoClient(mongoDBSettings.ConectionString);
                 return mongoClient.GetDatabase(serviceSettings.ServiceName);

             });
            
        return services;
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
