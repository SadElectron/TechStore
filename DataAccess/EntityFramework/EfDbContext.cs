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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        /*optionsBuilder.ConfigureWarnings(b => b.Log(
                (CoreEventId.StartedTracking, LogLevel.Information),
                (RelationalEventId.CommandExecuted, LogLevel.Information))).LogTo(Console.WriteLine, new[] {
            CoreEventId.StartedTracking,
            RelationalEventId.CommandExecuted
        },
            LogLevel.Debug, DbContextLoggerOptions.SingleLine);*/

        //optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionString"));

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Properties).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Details).WithOne(d => d.Product).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Images).WithOne(i => i.Product).HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductOrders).WithOne(op => op.Product).HasForeignKey(op => op.ProductId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Property>()
            .HasMany(p => p.Details).WithOne(d => d.Property).HasForeignKey(d => d.PropertyId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Order>()
            .HasMany(o => o.ProductOrders).WithOne(op => op.Order).HasForeignKey(op => op.OrderId).OnDelete(DeleteBehavior.NoAction);

    }
}
