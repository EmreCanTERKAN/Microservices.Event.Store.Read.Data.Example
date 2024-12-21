using EventStore.Client;

namespace Shared.Services.Abstractions
{
    public interface IEventStoreService
    {
        Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventData);
        EventData GenerateEventData(object @event);
    }
}
