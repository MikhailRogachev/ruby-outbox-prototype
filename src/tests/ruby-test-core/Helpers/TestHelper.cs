using AutoMapper;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Models;
using ruby_outbox_infrastructure.Profiles;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ruby_test_core.Helpers;

public static class TestHelper
{
    public static IMapper Mapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InfrastructureProfile>();
            cfg.AddProfile<RequestsProfile>();
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

    public static T GetOptionsFromAppSettings<T>()
    {
        FileInfo fi = new FileInfo("appsettings.json");
        var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        using StreamReader reader = new(fi.FullName);
        string body = reader.ReadToEnd();
        var jsonBody = JsonNode.Parse(body);
        var jsonBodyParameter = jsonBody![typeof(T).Name];

        return jsonBodyParameter!.Deserialize<T>(jsonOptions)!;
    }
}
