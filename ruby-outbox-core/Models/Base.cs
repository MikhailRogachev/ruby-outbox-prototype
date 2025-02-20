using ruby_outbox_core.Contracts.Interfaces;

namespace ruby_outbox_core.Models;

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
}
