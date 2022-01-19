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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var settingsFile = "appsettings.json";
            var key = "AppDbContext";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, true, true)
                .Build();
            
            var connectionString = config.GetConnectionString(key);
            if (connectionString == null)
            {
                throw new InvalidOperationException($"接続文字列が見つかりません。{settingsFile} がカレントディレクトリに存在しないか、ConnectionString セクションに {key} が存在しません。");
            }

            var serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));
            options.UseMySql(connectionString, serverVersion)
                   .LogTo(s => Debug.WriteLine(s), Microsoft.Extensions.Logging.LogLevel.Information)
                   .EnableSensitiveDataLogging()
                   .EnableDetailedErrors();
        }
    }
}
