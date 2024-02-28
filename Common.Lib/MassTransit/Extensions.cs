using Common.Lib.Settings;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {

            services
                .AddMassTransit(configure =>
                 {
                     //any consumers in same assembly will be registered
                     configure.AddConsumers(Assembly.GetEntryAssembly());

                     configure.UsingRabbitMq((context, configurator) =>
                     {

                         var config = context.GetService<IConfiguration>() ?? throw new ArgumentNullException(nameof(IConfiguration));

                         var serviceSettings = config
                             .GetSection(nameof(ServiceSettings))
                             .Get<ServiceSettings>();

                         var rabbitMQSettings = config
                             .GetSection(nameof(RabbitMQSettings))
                             .Get<RabbitMQSettings>();

                         //amqp://guest:guest@localhost:8187
                         var uri = new Uri($"amqp://{rabbitMQSettings?.Host}:{rabbitMQSettings?.Port}");

                         configurator.Host(uri, h =>
                         {
                             h.Username("guest");
                             h.Password("guest");
                         });

                         configurator
                             .ConfigureEndpoints(context,
                             new KebabCaseEndpointNameFormatter(serviceSettings?.ServiceName, false));
                         configurator.UseMessageRetry(retryConfig =>
                         {
                             retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                         });


                     });
                 });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}
