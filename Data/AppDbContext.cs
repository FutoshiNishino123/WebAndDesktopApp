using Data.Generators;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            
            var connectionString = config.GetConnectionString("AppDbContext");
            if (connectionString == null)
            {
                throw new InvalidOperationException($"ConnectionString is not found.");
            }

            var serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));

            options.UseMySql(connectionString, serverVersion)
                   .EnableDetailedErrors();

#if DEBUG
            options.LogTo(s => Debug.WriteLine(s), Microsoft.Extensions.Logging.LogLevel.Information)
                   .EnableSensitiveDataLogging();
#endif
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Order>()
                 .Property(o => o.Created)
                 .ValueGeneratedOnAdd()
                 .HasValueGenerator<CurrentTimeGenerator>();

            model.Entity<Order>()
                 .Property(o => o.Updated)
                 .ValueGeneratedOnAddOrUpdate()
                 .HasValueGenerator<CurrentTimeGenerator>();

            model.Entity<Order>()
                 .HasIndex(o => o.Number)
                 .IsUnique();

            model.Entity<Status>()
                 .HasIndex(s => s.Text)
                 .IsUnique();
        }
    }
}
