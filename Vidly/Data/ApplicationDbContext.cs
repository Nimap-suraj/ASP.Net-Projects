    using Microsoft.EntityFrameworkCore;
    using Vidly.Models;

    namespace Vidly.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
            {
            
            }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Movie> Movies { get; set; }
            public DbSet<MemberShipType> MemberShipType { get; set; }
            public DbSet<Genre> Genre { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Rental> Rental { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>().HasData
                    (
                        new User {Id=101, Email="admin@gmail.com",Password="1234",Role="admin" },
                        new User {Id=1, Email="suraj@gmail.com",Password="1234",Role="user" }
                    );
                base.OnModelCreating(modelBuilder);
            }
        }
    }
