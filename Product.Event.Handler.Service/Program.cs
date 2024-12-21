using Product.Event.Handler.Service.Services;
using Shared.Services;
using Shared.Services.Abstractions;

var builder = Host.CreateApplicationBuilder(args);

//background servisi belirtmemiz gerekmektedir.

builder.Services.AddHostedService<EventStoreBackgroundService>();

builder.Services.AddSingleton<IEventStoreService, EventStoreService>();
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

var host = builder.Build();
host.Run();
