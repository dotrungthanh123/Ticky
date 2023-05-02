using System.ComponentModel.DataAnnotations.Schema;

namespace Ticky.Models
{
    public class Retailer
    {
        public int RetailerId { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}