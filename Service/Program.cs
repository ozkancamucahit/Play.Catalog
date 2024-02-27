

using MongoDB.Driver;
using Service.Entities;
using Service.Helpers.Extensions;
using Service.Repositories;
using Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddMongo()
    .AddMongoRepository<Item>("items");
    
    


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
