using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Data
{
    public class AppDbContext : DbContext
    {
        #region Orders property
        private DbSet<Order>? orders;
        public DbSet<Order> Orders { get => orders ?? throw new InvalidOperationException(); set => orders = value; }
        #endregion

        #region People property
        private DbSet<Person>? people;
        public DbSet<Person> People { get => people ?? throw new InvalidOperationException(); set => people = value; }
        #endregion

        #region Statuses property
        private DbSet<Status>? statuses;
        public DbSet<Status> Statuses { get => statuses ?? throw new InvalidOperationException(); set => statuses = value; }
        #endregion

        public string ConnectionString { get; private set; }

        public AppDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public AppDbContext()
        {
            // NOTE: appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            ConnectionString = config.GetConnectionString("AppDbContext");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = ConnectionString;
            var serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));

            options.UseMySql(connectionString, serverVersion)
                   .LogTo(s => Debug.WriteLine(s), Microsoft.Extensions.Logging.LogLevel.Information)
                   .EnableSensitiveDataLogging()
                   .EnableDetailedErrors();
        }
    }
}
