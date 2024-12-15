using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DatabaseContext;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<City> Cities { get; set; }

    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>().HasData(
            new City
            {
                Id = Guid.Parse("AD49CF1F-ED27-4D38-A4F3-3ED3006E208A"),
                Name = "Tokyo"
            }
        );
        modelBuilder.Entity<City>().HasData(
            new City
            {
                Id = Guid.Parse("1634CA72-4E8A-4151-BD97-3526909D62CB"),
                Name = "New York"
            }
        );
    }
}