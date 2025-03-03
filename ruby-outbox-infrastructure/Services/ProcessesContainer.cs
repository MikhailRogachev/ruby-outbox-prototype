using Microsoft.Extensions.DependencyInjection;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events;
using ruby_outbox_infrastructure.Processes;

namespace ruby_outbox_infrastructure.Services;

public static class ProcessesContainer
{
    public static void Init(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEventHandler<StartVmCreation>, CreateVmProcess>();
        serviceCollection.AddScoped<IEventHandler<CreateNic>, CreateVmProcess>();
        serviceCollection.AddScoped<IEventHandler<CreateVmResource>, CreateVmProcess>();
    }
}