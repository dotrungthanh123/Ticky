namespace Ticky.Models
{
    public interface ICustomerRepository
    {
        public Customer? GetCustomerByAccountId(int accountId);
    }
}
