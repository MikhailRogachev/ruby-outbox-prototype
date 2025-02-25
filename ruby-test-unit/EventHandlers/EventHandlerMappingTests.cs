using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events;
using ruby_outbox_core.Models;
using ruby_outbox_data.Extensions;
using ruby_test_core.Attributes;
using System.Text.Json;

namespace ruby_test_unit.EventHandlers;

public class EventHandlerMappingTests
{
    public static OutboxMessage GetMessage()
    {
        var @event = new StartVmCreation { EventId = Guid.NewGuid(), VmId = Guid.NewGuid(), CreatedAt = DateTime.Now };
        var src = new List<IEvent> { @event };
        return OutboxMessageBuilder.GetOutboxData(src).First();
    }

    [Theory, AutoMock]
    public void EventHandlerDefinitionTest(
        [RegInstance(nameof(GetMessage))] OutboxMessage message
        )
    {
        var type = TryGetType(message.ContentType);
        var @event = JsonSerializer.Deserialize(message.Content!, type!);

        //var cls = Activator.CreateInstance()


    }


    private Type? TryGetType(string typeName)
    {

        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == typeof(IEvent).Assembly.GetName().Name);

        return assembly!.GetTypes().FirstOrDefault(p => p.Name == typeName);
    }
}
