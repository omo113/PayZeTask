namespace PayZe.Shared.Abstractions;

public interface IHasDomainEvent
{
    IReadOnlyList<DomainEvent> PendingDomainEvents();
}

public abstract class DomainEvent
{
    public Guid UId { get; set; }
    public DateTimeOffset DateOccurred { get; protected set; } = SystemDate.Now;
    public int Version { get; internal set; }
}
