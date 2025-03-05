using AutoMapper;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Models;
using ruby_outbox_infrastructure.Profiles;

namespace ruby_test_unit.Helpers;

public static class TestHelper
{
    public static IMapper Mapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InfrastructureProfile>();
        });

        return new Mapper(config);
    }

    public static Customer GetCustomer() => new Customer();

    public static Vm GetVm(Guid? customerId = null, VmStatus vmStatus = VmStatus.NotStarted)
    {
        var vm = new Vm(customerId ?? Guid.NewGuid());
        vm.Status = vmStatus;

        return vm;
    }
}
