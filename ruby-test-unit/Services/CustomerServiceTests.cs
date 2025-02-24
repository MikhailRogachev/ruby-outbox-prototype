using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Moq;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;
using ruby_outbox_infrastructure.Services;
using ruby_test_core.Attributes;
using ruby_test_unit.Helpers;

namespace ruby_test_unit.Services;

public class CustomerServiceTests
{
    public static IMapper Mapper() => TestHelper.Mapper();
    public static Customer GetCustomer() => TestHelper.GetCustomer();

    [Theory, AutoMock]
    public async Task GetCustomerDto_Result(
        Guid customerId,
        [RegInstance(nameof(GetCustomer))] Customer customer,
        [Frozen] Mock<ICustomerRepository> repository,
        [Frozen, RegInstance(nameof(Mapper))]
        CustomerService sut
        )
    {
        // arrange
        repository.Setup(p => p.TryGetAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

        // act
        var response = await sut.GetCustomerDtoAsync(customerId, CancellationToken.None);

        // assert
        response.Should().NotBeNull().And.BeAssignableTo<CustomerDto>();
    }

    [Theory, AutoMock]
    public async Task GetCustomerDto_Null(
        Guid customerId,
        [Frozen] Mock<ICustomerRepository> repository,
        CustomerService sut
        )
    {
        // arrange
        repository.Setup(p => p.TryGetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Customer));

        // act
        var response = await sut.GetCustomerDtoAsync(customerId, CancellationToken.None);

        // assert
        response.Should().BeNull();
    }

    [Theory, AutoMock]
    public async Task GetCustomer_Result(
        Guid customerId,
        [RegInstance(nameof(GetCustomer))] Customer customer,
        [Frozen] Mock<ICustomerRepository> repository,
        CustomerService sut
        )
    {
        // arrange
        repository.Setup(p => p.TryGetAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

        // act
        var response = await sut.GetCustomerAsync(customerId, CancellationToken.None);

        // assert
        response.Should().NotBeNull().And.BeAssignableTo<Customer>();
    }

    [Theory, AutoMock]
    public async Task GetCustomer_Null(
        Guid customerId,
        [Frozen] Mock<ICustomerRepository> repository,
        CustomerService sut
        )
    {
        // arrange
        repository.Setup(p => p.TryGetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Customer));

        // act
        var response = await sut.GetCustomerAsync(customerId, CancellationToken.None);

        // assert
        response.Should().BeNull();
    }

    [Theory, AutoMock]
    public async Task AddCustomer_Result(
        CustomerDto dto,
        [RegInstance(nameof(GetCustomer))] Customer customer,
        [Frozen] Mock<ICustomerRepository> repository,
        CustomerService sut
        )
    {
        // arrange
        repository.Setup(p => p.TryGetAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

        // act
        var response = await sut.AddCustomerAsync(dto, CancellationToken.None);

        // assert
        response.Should().NotBeNull().And.BeAssignableTo<CustomerDto>();
    }

    [Theory, AutoMock]
    public async Task AddCustomer_Exception(
        CustomerDto dto,
        [Frozen] Mock<ICustomerRepository> repository,
        CustomerService sut
        )
    {
        // arrange
        repository.Setup(p => p.TryGetAsync(It.IsAny<Guid>())).ReturnsAsync(default(Customer));

        // act
        Func<Task> func = async () => await sut.AddCustomerAsync(dto, CancellationToken.None);

        // assert
        await func.Should().ThrowAsync<Exception>();
    }
}
