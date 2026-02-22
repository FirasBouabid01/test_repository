using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use PostgreSQL connection string
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=test;Username=postgres;Password=123456789",
            b => b.MigrationsAssembly("Infrastructure")
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
