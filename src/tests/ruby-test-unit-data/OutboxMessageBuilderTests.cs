using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Events.CreateVm;
using ruby_outbox_data.Extensions;
using ruby_test_core.Attributes;

namespace ruby_test_unit_data;

public class OutboxMessageBuilderTests
{
    public static IEnumerable<IEvent> Events()
    {
        return new List<IEvent>() { new CreateNic { VmId = Guid.NewGuid() } };
    }


    [Theory, AutoMock]
    public void GetOutboxDataTest(
        [RegInstance(nameof(Events))] IEnumerable<IEvent> events
        )
    {
        // act
        var outboxMessage = OutboxMessageBuilder.GetOutboxData(events);

        // assert
        Assert.Equal(events.Count(), outboxMessage.Count());
        Assert.Equal(typeof(CreateNic).Name, outboxMessage.First().ContentType);
        Assert.Equal(OutboxMessageStatus.Ini, outboxMessage.First().Status);
    }
}
