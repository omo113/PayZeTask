using PayZe.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PayZe.Shared.Abstractions;

public abstract class Entity
{
    [Key]
    public int Id { get; protected init; }

    public Guid UId { get; protected init; }

    public EntityStatus EntityStatus { get; private set; } = EntityStatus.Active;
    public DateTimeOffset CreateDate { get; protected init; } = SystemDate.Now;

    [ConcurrencyCheck]
    public DateTimeOffset? LastChangeDate { get; protected set; }

    protected Entity()
    {

    }

    internal void Restore()
    {
        EntityStatus = EntityStatus.Active;
    }

    public virtual void Delete()
    {
        LastChangeDate = SystemDate.Now;
        EntityStatus = EntityStatus.Deleted;
    }

    public bool Active()
    {
        return EntityStatus == EntityStatus.Active;
    }
}