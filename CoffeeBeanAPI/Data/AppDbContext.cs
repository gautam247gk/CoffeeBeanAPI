using CoffeeBeanAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeBeanAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Bean> Beans { get; set; }
        public DbSet<BeanOfTheDay> BeanOfTheDays { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BeanOfTheDay>()
                .HasIndex(b => b.SelectedDate)
                .IsUnique();

            modelBuilder.Entity<Bean>()
                .Property(b => b.Cost)
                .HasPrecision(18, 2);
        }
    }

}



