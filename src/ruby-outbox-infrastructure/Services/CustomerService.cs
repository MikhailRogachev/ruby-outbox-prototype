using AutoMapper;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Services;

public class CustomerService(
    ILogger<CustomerService> logger,
    ICustomerRepository repository,
    IMapper mapper
    ) : ICustomerService
{
    public async Task<CustomerDto> AddCustomerAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(customerDto, opt => opt.AfterMap((src, dest) => { dest.Comment = "New Customer added."; }));
        repository.Add(customer);

        await repository.UnitOfWork.SaveAsync(cancellationToken);

        var dto = await GetCustomerDtoAsync(customerDto.CustomerId, cancellationToken);

        if (dto == null)
        {
            logger.LogInformation("The customer {cid} hasn't been created.", customerDto.CustomerId);
            throw new Exception($"The customer {customerDto.CustomerId} hasn't been created.");
        }

        logger.LogInformation("The customer {cid} has been created successfully.", customerDto.CustomerId);
        return dto;
    }

    public async Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken)
    {
        logger.LogInformation("The {sn} retrieve customer {cid}.", nameof(CustomerService), customerId);

        var customer = await repository.TryGetAsync(customerId);

        if (customer == null)
            logger.LogInformation("Customer: {cid} is not found.", customerId);

        return customer;
    }

    public async Task<CustomerDto?> GetCustomerDtoAsync(Guid customerId, CancellationToken cancellationToken)
    {
        logger.LogInformation("The {sn} retrieve customer dto {cid}.", nameof(CustomerService), customerId);

        var customer = await repository.TryGetAsync(customerId);

        if (customer != null)
            return mapper.Map<CustomerDto>(customer);

        logger.LogInformation("Customer: {cid} is not found.", customerId);

        return null;
    }
}
