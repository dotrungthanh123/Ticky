using Microsoft.EntityFrameworkCore;
using Ticky.Models;

namespace Ticky.Data
{
    public class TickyContext : DbContext
    {
        public TickyContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<TicketOrder> TicketOrders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Retailer> Retailers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
            .HasIndex(u => u.Username)
            .IsUnique();
        }
    }
}