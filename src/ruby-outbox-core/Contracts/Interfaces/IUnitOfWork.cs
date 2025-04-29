namespace ruby_outbox_core.Contracts.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
}
