using System.Text.Json.Serialization;
using CleanArch.Domain.IntegrationEvents.Interfaces;

namespace CleanArch.Domain.IntegrationEvents;
public record IntegrationEvent : IIntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }
    
}