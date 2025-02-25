using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Repositories;

public interface IOutboxMessageRepository
{
    Task<OutboxMessage?> GetMessageToProc();
    Task<bool> UpdateAsync(Guid eventId, OutboxMessageStatus status, string message);
}
