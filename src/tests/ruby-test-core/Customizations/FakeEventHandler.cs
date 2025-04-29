using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_infrastructure.EventHandlers;

namespace ruby_test_core.Customizations;

public class FakeEventHandler : BaseEventHandler, IEventHandler<FakeEvent>
{

    [ActivatorUtilitiesConstructor]
    public FakeEventHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task HandleAsync(FakeEvent @event)
    {
    }
}
