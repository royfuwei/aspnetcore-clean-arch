using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Domain.SeedWork.Interfaces;

public interface IEntityBase
{
    public int Id { get; set;}
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    public void AddDomainEvent(DomainEvent eventItem);
    public void RemoveDomainEvent(DomainEvent eventItem);
    public void ClearDomainEvents();
    public bool IsTransient();
}
