using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_data.Repositories;

public class OutboxRepository(
    ILogger<OutboxRepository> logger,
    ApplicationDbContext context
    ) : IOutboxMessageRepository
{
    public async Task<OutboxMessage?> GetMessageToProc()
    {
        var message = await context.OutboxMessages.OrderBy(p => p.CreationDate).FirstOrDefaultAsync(p => p.Status != OutboxMessageStatus.Locked);

        if (message == null)
            return message;

        message.Status = OutboxMessageStatus.Locked;
        message.LastModifiedDate = DateTime.UtcNow;
        message.Message = "start process";

        await context.SaveAsync();
        return message;
    }

    public Task<bool> UpdateAsync(Guid eventId, OutboxMessageStatus status, string message)
    {
        throw new NotImplementedException();
    }
}
