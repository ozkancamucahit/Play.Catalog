

using MongoDB.Driver;
using Service.Repositories;
using Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ServiceSettings serviceSettings;

serviceSettings = builder
    .Configuration
    .GetSection(nameof(ServiceSettings))
    .Get<ServiceSettings>() ?? throw new ArgumentNullException(nameof(ServiceSettings));


builder
    .Services
    .AddSingleton(serviceProvider =>
    {
        var mongoDBSettings = builder
            .Configuration
            .GetSection(nameof(MongoDBSettings))
            .Get<MongoDBSettings>() ?? throw new ArgumentNullException(nameof(MongoDBSettings));

        var mongoClient = new MongoClient(mongoDBSettings.ConectionString);
        return mongoClient.GetDatabase(serviceSettings.ServiceName);

    })
    .AddSingleton<IItemRepository, ItemRepository>();


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
