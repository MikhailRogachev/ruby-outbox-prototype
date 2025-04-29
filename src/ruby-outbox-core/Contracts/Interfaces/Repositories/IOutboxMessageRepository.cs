using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Repositories;

public interface IOutboxMessageRepository
{
    IUnitOfWork UnitOfWork { get; }
    Task<OutboxMessage?> GetMessageToProc();
    Task<OutboxMessage?> GetMessageById(Guid eventId);
    OutboxMessage Update(OutboxMessage outboxMessage);
    void Remove(OutboxMessage outboxMessage);
}
