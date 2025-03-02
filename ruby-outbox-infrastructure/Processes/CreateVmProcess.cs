using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events;

namespace ruby_outbox_infrastructure.Processes;

public class CreateVmProcess :
    IEventHandler<StartVmCreation>,
    IEventHandler<CreateNic>
{
    public async Task HandleAsync(StartVmCreation @event)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(CreateNic @event)
    {
        throw new NotImplementedException();
    }
}
