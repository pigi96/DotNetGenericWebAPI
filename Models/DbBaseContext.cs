using Microsoft.EntityFrameworkCore;

namespace GenericWebAPI.Models;

public class DbBaseContext : DbContext
{
    public DbBaseContext(DbContextOptions options) : base(options)
    {
        // Allow program.cs configuration
    }

    public DbBaseContext()
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // No base entity ideas
    }
}