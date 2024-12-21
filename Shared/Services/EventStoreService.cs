﻿using EventStore.Client;
using Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class EventStoreService : IEventStoreService
    {
        EventStoreClientSettings GetEventStoreClientSettings(string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false") => EventStoreClientSettings.Create(connectionString);

        EventStoreClient Client { get => new EventStoreClient(GetEventStoreClientSettings()); }

        public async Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventData)
            => await Client.AppendToStreamAsync(
                streamName: streamName,
                eventData: eventData,
                expectedState: StreamState.Any
                );



        public EventData GenerateEventData(object @event) => new(
            eventId: Uuid.NewUuid(),
            type: @event.GetType().Name,
            data: JsonSerializer.SerializeToUtf8Bytes(@event));

    }
}
