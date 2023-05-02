using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticky.Models
{
    [PrimaryKey(nameof(EventId), nameof(OrderId))]
    public class OrderDetail
    {
        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int OrderId { get; set; }
        public TicketOrder? TicketOrder { get; set; }

        public int Quantity { get; set; }
    }
}