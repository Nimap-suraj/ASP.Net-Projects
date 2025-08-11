using Microsoft.EntityFrameworkCore;
using Serene.Entity;

namespace Serene.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }  
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Appointment>().Property(x => x.Price).HasPrecision(10, 2);
        }

    }
}
