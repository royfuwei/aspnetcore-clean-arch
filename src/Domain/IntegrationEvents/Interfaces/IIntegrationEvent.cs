namespace CleanArch.Domain.IntegrationEvents.Interfaces;
public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime CreationDate { get;}
}