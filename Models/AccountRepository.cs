using Microsoft.EntityFrameworkCore;
using Ticky.Data;

namespace Ticky.Models
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TickyContext _TickyDbContext;

        public AccountRepository(TickyContext TickyContext)
        {
            _TickyDbContext = TickyContext;
        }

        public void addAccount(Account account)
        {
            _TickyDbContext.Add(account);
            _TickyDbContext.SaveChanges();
        }

        public Account? GetAccountWithUsernameAndPassword(string username, string password)
        {
            return _TickyDbContext.Accounts.SingleOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
