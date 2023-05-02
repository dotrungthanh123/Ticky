using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticky.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public List<Retailer> Retailers { get; } = new List<Retailer>();
        public List<Event> Events { get; } = new List<Event>();

        public string Name { get; set; }
        public float Salary { get; set; }
    }
}