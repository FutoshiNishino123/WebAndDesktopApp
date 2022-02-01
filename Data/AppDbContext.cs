using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    // 作成・更新日を設定する

        //    var entries = ChangeTracker.Entries()
        //                               .Where(e => e.Entity is ITimeStamp
        //                                           && (e.State == EntityState.Added
        //                                           || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        var entity = (ITimeStamp)entityEntry.Entity;

        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            entity.Created = DateTime.Now;
        //        }

        //        entity.Updated = DateTime.Now;
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }

    public class CurrentTimeGenerator : ValueGenerator<DateTime>
    {
        public override bool GeneratesTemporaryValues => false;

        public override DateTime Next(EntityEntry entry) => DateTime.Now;

        protected override object NextValue(EntityEntry entry) => DateTime.Now;
    }
}
