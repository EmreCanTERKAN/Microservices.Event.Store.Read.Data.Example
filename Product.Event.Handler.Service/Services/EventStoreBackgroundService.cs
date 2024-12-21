using MongoDB.Driver;
using Shared.Events;
using Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Product.Event.Handler.Service.Services
{
    public class EventStoreBackgroundService(IEventStoreService eventStoreService, IMongoDbService mongoDbService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventStoreService.SubscribeToStreamAsync(streamName: "products-stream", eventAppeared: async (streamSubscription, resolvedEvent, cancellationToken) =>
            {
                string eventType = resolvedEvent.Event.EventType;
                object @event = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(), Assembly.Load("Shared").GetTypes().FirstOrDefault(t => t.Name == eventType));

                //mongodb collection
                var productCollection = mongoDbService.GetCollection<Models.Product>("products");

                switch (@event)
                {
                    case NewProductAddedEvent newProductAddedEvent:
                        //Buradaki FindAsync MongoDb driverdan gelmeli.
                        var hasProduct =await (await productCollection.FindAsync(p => p.Id == newProductAddedEvent.ProductId)).AnyAsync();
                        if (!hasProduct)
                            await productCollection.InsertOneAsync(new()
                            {
                                Id = newProductAddedEvent.ProductId,
                                ProductName = newProductAddedEvent.ProductName,
                                Count = newProductAddedEvent.InitialCount,
                                IsAvailable = newProductAddedEvent.IsAvailable,
                                Price = newProductAddedEvent.InitialPrice
                            });
                        break;


                    default:
                        break;
                }
            });
        }
    }
}
