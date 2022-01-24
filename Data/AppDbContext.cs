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

        #region People property
        private DbSet<Person>? _people;
        public DbSet<Person> People
        {
            get => _people ?? throw new NullReferenceException();
            set => _people = value;
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
    }
}
