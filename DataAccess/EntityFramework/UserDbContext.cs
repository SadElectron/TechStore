using Core.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework;

public class UserDbContext : IdentityDbContext<CustomIdentityUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(b => b.Log(
               (CoreEventId.StartedTracking, LogLevel.Information),
               (RelationalEventId.CommandExecuted, LogLevel.Information)))
            .LogTo(Console.WriteLine, new[] {
            CoreEventId.StartedTracking,
            RelationalEventId.CommandExecuted
       }, LogLevel.Debug, DbContextLoggerOptions.SingleLine);

        optionsBuilder.EnableSensitiveDataLogging();
        var dbPath = Path.Combine(AppContext.BaseDirectory, "DataAccess", "User.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath};Cache=Shared");
    }
}
