using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface ICustomerService
{
    Task<CustomerDto?> GetCustomerDtoAsync(Guid customerId, CancellationToken cancellationToken);
    Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken);
    Task<CustomerDto> AddCustomerAsync(CustomerDto customerDto, CancellationToken cancellationToken);
}
