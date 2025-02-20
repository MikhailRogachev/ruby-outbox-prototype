using ruby_outbox_core.Models;

namespace ruby_outbox_core.Contracts.Interfaces.Repositories;

public interface ICustomerRepository
{
    Customer Add(Customer customer);
    void Remove(Customer customer);
    Customer Update(Customer customer);
    Task<Customer?> TryGetAsync(Guid Id);
}
