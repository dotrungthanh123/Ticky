using System.ComponentModel.DataAnnotations.Schema;

namespace Ticky.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public List<TicketOrder> ticketOrders { get; } = new List<TicketOrder>();

        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}