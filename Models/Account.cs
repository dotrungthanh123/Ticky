using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ticky.Models
{
    [PrimaryKey(nameof(AccountId))]
    public class Account
    {
        public int Role { get; set; }
        public Admin? Admin { get; set; }
        public Retailer? Retailer { get; set; }
        public Customer? Customer { get; set; }
        public int AccountId { get; set; }
        [StringLength(450)]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
