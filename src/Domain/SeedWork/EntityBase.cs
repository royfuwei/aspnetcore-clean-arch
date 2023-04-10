using CleanArch.Domain.SeedWork.Interfaces;

namespace CleanArch.Domain.SeedWork;

public abstract class EntityBase : IEntityBase
{
    int? _requestedHashCode;
    public int Id { get; set;}
    private List<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent eventItem) => _domainEvents.Add(eventItem);

    public void RemoveDomainEvent(DomainEvent eventItem) => _domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => _domainEvents.Clear();

    public bool IsTransient() => this.Id == default(Int32);

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is EntityBase)) return false;
        if (Object.ReferenceEquals(this, obj)) return true;
        
        EntityBase item = (EntityBase)obj;

        if (item.IsTransient() || this.IsTransient()) return false;

        return item.Id == this.Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();

    }

    /// <summary>
    /// 定義在C#中的類別中的比較運算子 "==" 的實作
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(EntityBase left, EntityBase right)
    {
        if (Object.Equals(left, null))
            return (Object.Equals(right, null)) ? true : false;
        else
            return left.Equals(right);
    }

    /// <summary>
    /// 定義在C#中的類別中的比較運算子 "!=" 的實作
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(EntityBase left, EntityBase right)
    {
        return !(left == right);
    }

}