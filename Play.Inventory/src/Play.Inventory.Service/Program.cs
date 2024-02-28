



using Common.Lib.MassTransit;
using Common.Lib.MongoDB;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddMongo()
    .AddMongoRepository<InventoryItem>("inventoryitems")
    .AddMongoRepository<CatalogItem>("catalogitems")
    .AddMassTransitWithRabbitMQ()
    .AddHttpClient<CatalogClient>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5034/api");

    })
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(2));


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
