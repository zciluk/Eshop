using Eshop.Domain.Customers;
using Eshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Eshop.Infrastructure.Repositories
{
  internal class CustomerRepository : ICustomerRepository
  {
    private readonly CustomersContext _context;
    private readonly IEntityTracker _entityTracker;

    public CustomerRepository(CustomersContext context, IEntityTracker entityTracker)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _entityTracker = entityTracker ?? throw new ArgumentNullException(nameof(entityTracker));
    }

    public void Add(Customer customer)
    {
      _context.Customers.InsertOne(customer);
      _entityTracker.TrackEntity(customer);
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
      var Customer = await _context.Customers.Find(c => c.Id == id).FirstAsync();

      if (Customer == null)
      {
        throw new CustomerNotExistsException(id);
      }

      _entityTracker.TrackEntity(Customer);

      return Customer;
    }
  }
}
