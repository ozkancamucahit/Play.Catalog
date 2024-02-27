﻿using Common.Lib.Repositories;
using Common.Lib.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Common.Lib.MongoDB;

public static class Extensions
{

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



}
