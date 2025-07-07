using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Models;

/// <summary>
///     Represents a base entity with common properties and functionality for domain models.
/// </summary>
/// <remarks>
///     The <see cref="Base"/> class provides standard properties such as <see cref="Id"/>,  
///     <see cref="CreatedAt"/>, and <see cref="UpdatedAt"/> for tracking entity metadata.  
///     It also includes functionality for managing domain events through the <see cref="Events"/> 
///     collection.
/// </remarks>
public class Base
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Comment { get; set; }

    private List<IEvent> _events = new();
    public IReadOnlyCollection<IEvent> Events => _events;

    public Base()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void RemoveAllEvents()
    {
        _events.Clear();
    }

    public void AddEvent(IEvent @event)
    {
        _events.Add(@event);
    }
}
