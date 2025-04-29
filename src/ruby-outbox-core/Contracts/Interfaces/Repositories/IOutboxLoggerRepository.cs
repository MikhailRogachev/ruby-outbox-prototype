using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Repositories;

public interface IOutboxLoggerRepository
{
    IUnitOfWork UnitOfWork { get; }
    OutboxErrorLogger CreateRecord(OutboxErrorLogger record);
}
