using AutoMapper;
using FluentAssertions;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;
using ruby_test_core.Attributes;
using ruby_test_unit.Helpers;

namespace ruby_test_unit.Profiles;

public class MapperProfileTests
{

    public static IMapper Mapper() => TestHelper.Mapper();
    public static Customer GetCustomer() => TestHelper.GetCustomer();

    [Theory, AutoMock]
    public void CustomerMapper_Test(
        CustomerDto dto,
        [RegInstance(nameof(Mapper))] IMapper mapper
        )
    {
        // act
        var customer = mapper.Map<Customer>(dto);

        // assert
        customer.Id.Should().Be(dto.CustomerId);
    }

    [Theory, AutoMock]
    public void CustomerDtoMapper_Test(
        [RegInstance(nameof(GetCustomer))] Customer customer,
        [RegInstance(nameof(Mapper))] IMapper mapper
        )
    {
        // act
        var dto = mapper.Map<CustomerDto>(customer);

        // assert
        dto.CustomerId.Should().Be(customer.Id);
    }
}
