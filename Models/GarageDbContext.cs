using Microsoft.EntityFrameworkCore;

namespace garagebackend.Models
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options) 
        { 
        }

        public DbSet<garagebackend.Models.Credentials> Credentials { get; set; } = default!;
        public DbSet<garagebackend.Models.Contacts> Contacts { get; set; } = default!;
    }
}
