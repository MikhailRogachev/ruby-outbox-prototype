using AutoMapper;
using FluentAssertions;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;
using ruby_test_core.Attributes;
using ruby_test_core.Helpers;

namespace ruby_test_unit.Profiles;

public class VmMappingTest
{
    public static IMapper Mapper() => TestHelper.Mapper();
    public static Vm GetVm() => new Vm(Guid.NewGuid());

    [Theory, AutoMock]
    public void VmDtoMappingTest(
        [RegInstance(nameof(GetVm))] Vm vm,
        [RegInstance(nameof(Mapper))] IMapper mapper
        )
    {
        // act
        var dto = mapper.Map<VmDto>(vm);

        // assert
        dto.Id.Should().Be(vm.Id);
        dto.CustomerId.Should().Be(vm.CustomerId!.Value);
    }

}
