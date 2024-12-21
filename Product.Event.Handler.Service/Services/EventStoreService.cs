using Shared.Events;
using Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Product.Event.Handler.Service.Services
{
    public class EventStoreService(IEventStoreService eventStoreService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventStoreService.SubscribeToStreamAsync(streamName: "products-stream", eventAppeared: async (streamSubscription, resolvedEvent, cancellationToken) =>
            {
                string eventType = resolvedEvent.Event.EventType;
                object @event = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(), Assembly.Load("Shared").GetTypes().FirstOrDefault(t => t.Name == eventType));
                switch (@event)
                {
                    case NewProductAddedEvent newProductAddedEvent:
                        // Do something with the event
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
