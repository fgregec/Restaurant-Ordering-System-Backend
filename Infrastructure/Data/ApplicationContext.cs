using Core.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<GuestOrder> GuestOrder { get; set; }
    }
}
