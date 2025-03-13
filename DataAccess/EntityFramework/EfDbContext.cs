using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection.Metadata;


namespace DataAccess.EntityFramework;

public class EfDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Order> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Detail> Details { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(b => b.Log(
                (CoreEventId.StartedTracking, LogLevel.Information),
                (RelationalEventId.CommandExecuted, LogLevel.Information))).LogTo(Console.WriteLine, new[] {
            CoreEventId.StartedTracking,
            RelationalEventId.CommandExecuted
        },
            LogLevel.Debug, DbContextLoggerOptions.SingleLine);

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSqlite($"Data Source=../DataAccess/Application.db;Cache=Shared");

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
