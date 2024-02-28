

using Common.Lib.MongoDB;
using Common.Lib.Settings;
using MassTransit;
using MassTransit.Definition;
using Service.Entities;
using Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddMongo()
    .AddMongoRepository<Item>("items");


builder
    .Services
    .AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, configurator) =>
        {
            var serviceSettings = builder
                .Configuration
                .GetSection(nameof(ServiceSettings))
                .Get<ServiceSettings>();

            var rabbitMQSettings = builder
                .Configuration
                .GetSection(nameof(RabbitMQSettings))
                .Get<RabbitMQSettings>();

            //amqp://guest:guest@localhost:8187
            var uri = new Uri($"amqp://{rabbitMQSettings?.Host}:{rabbitMQSettings?.Port}");

            configurator.Host(uri, h => {
                h.Username("guest");
                h.Password("guest");
            });

            configurator
                .ConfigureEndpoints(context,
                new KebabCaseEndpointNameFormatter(serviceSettings?.ServiceName, false));

        });
    });
builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
