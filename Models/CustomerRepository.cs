using Ticky.Data;

namespace Ticky.Models
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TickyContext _TickyDbContext;

        public CustomerRepository(TickyContext TickyContext)
        {
            _TickyDbContext = TickyContext;
        }

        public Customer? GetCustomerByAccountId(int accountId)
        {
            return _TickyDbContext.Customers.SingleOrDefault(c => c.AccountId == accountId);
        }
    }
}
