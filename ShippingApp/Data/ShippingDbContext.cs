using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Models;
using ShippingApp.Services;
using System.Text;

namespace ShippingApp.Data
{
    public class ShippingDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        SecondaryAuthService _secondaryAuthService;

        public ShippingDbContext(IConfiguration configuration,DbContextOptions options) : base(options)
        {
            _configuration= configuration;
            _secondaryAuthService = new SecondaryAuthService(configuration);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User(Guid.NewGuid(), "Admin", "Admin", "admin@gmail.com", 9865326598, "address", _secondaryAuthService.CreatePasswordHash("Admin@123"), "pathToProfilePic", "admin", ""));
        }
        public DbSet<User> Users { get; set; }
    }
}
