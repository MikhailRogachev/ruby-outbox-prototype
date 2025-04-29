using Microsoft.EntityFrameworkCore;
using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_data.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Customer Add(Customer customer)
    {
        return context.Add(customer).Entity;
    }

    public void Remove(Customer customer)
    {
        context.Customers.Remove(customer);
    }

    public Customer Update(Customer customer)
    {
        return context.Update(customer).Entity;
    }

    public async Task<Customer?> TryGetAsync(Guid Id)
    {
        return await context.Customers.FirstOrDefaultAsync(p => p.Id == Id, CancellationToken.None);
    }
}
