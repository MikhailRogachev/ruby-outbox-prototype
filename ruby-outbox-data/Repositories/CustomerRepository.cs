using ruby_outbox_core.Contracts.Interfaces;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Models;
using ruby_outbox_data.Persistency;
using System.Data.Entity;

namespace ruby_outbox_data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Customer Add(Customer customer)
    {
        return _context.Add(customer).Entity;
    }

    public void Remove(Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public Customer Update(Customer customer)
    {
        return _context.Update(customer).Entity;
    }

    public async Task<Customer?> TryGetAsync(Guid Id)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.Id == Id);
    }
}
