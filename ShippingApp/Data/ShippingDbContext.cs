using Microsoft.EntityFrameworkCore;
using ShippingApp.Models;

namespace ShippingApp.Data
{
    public class ShippingDbContext : DbContext
    {
        public ShippingDbContext()
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
