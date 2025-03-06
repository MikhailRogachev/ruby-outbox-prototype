using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_data.Repositories;

public class OutboxLoggerRepository(ApplicationDbContext context) : IOutboxLoggerRepository
{
    public IUnitOfWork UnitOfWork => context;

    public OutboxErrorLogger CreateRecord(OutboxErrorLogger record)
    {
        return context.Add(record).Entity;
    }
}
