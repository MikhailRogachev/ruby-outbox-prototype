using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Repositories;

public interface IOutboxMessageRepository
{
    IUnitOfWork UnitOfWork { get; }
    Task<OutboxMessage?> GetMessageToProc();
    Task<OutboxMessage?> GetMessageById(Guid eventId);
    Task<bool> UpdateAsync(Guid eventId, OutboxMessageStatus status, string message);
    void Remove(OutboxMessage outboxMessage);
}
