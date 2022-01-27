using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Data
{
    public class AppDbContext : DbContext
    {
        #region Orders property
        private DbSet<Order>? _orders;
        public DbSet<Order> Orders
        {
            get => _orders ?? throw new NullReferenceException();
            set => _orders = value;
        }
        #endregion

        #region Users property
        private DbSet<User>? _users;
        public DbSet<User> Users
        {
            get => _users ?? throw new NullReferenceException();
            set => _users = value;
        }
        #endregion

        #region Statuses property
        private DbSet<Status>? _statuses;
        public DbSet<Status> Statuses
        {
            get => _statuses ?? throw new NullReferenceException();
            set => _statuses = value;
        }
        #endregion

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
                   .LogTo(s => Debug.WriteLine(s), Microsoft.Extensions.Logging.LogLevel.Information)
                   .EnableSensitiveDataLogging()
                   .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Order>()
                 .Property(o => o.Created)
                 .HasDefaultValueSql("NOW()");

            model.Entity<Order>()
                 .Property(o => o.Updated)
                 .HasDefaultValueSql("NOW()");

            model.Entity<Order>()
                 .HasIndex(o => o.Number)
                 .IsUnique();

            model.Entity<User>()
                 .HasIndex(u => u.EmailAddress)
                 .IsUnique();

            model.Entity<Status>()
                 .HasIndex(s => s.Text)
                 .IsUnique();
        }
    }
}
