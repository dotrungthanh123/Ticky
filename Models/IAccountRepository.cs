namespace Ticky.Models
{
    public interface IAccountRepository
    {
        public Account? GetAccountWithUsernameAndPassword(string username, string password);
        public void addAccount(Account account);
    }
}
