using Product.Event.Handler.Service.Services;
using Shared.Services.Abstractions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IEventStoreService, IEventStoreService>();

var host = builder.Build();
host.Run();
