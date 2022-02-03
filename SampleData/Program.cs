using Common.Utils;
using Data;
using Data.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SampleData;

InitializeDb();

void InitializeDb()
{
    // 最初にDataプロジェクトからデータベースをマイグレーションすること
    // `dotnet ef add migrations Sample`
    // `dotnet ef database update`

    Console.Write("すべてのデータを削除して新しいデータを登録します。続行しますか？(y/n): ");
    var key = Console.ReadKey().Key;
    Console.WriteLine();
    if (key != ConsoleKey.Y)
    {
        Console.WriteLine("キャンセルされました。");
        return;
    }

    Console.WriteLine("Deleting All Data...");
    DeleteAllData();
    Console.WriteLine("Done.");

    using var db = new AppDbContext();
    var users = new SampleUsers(db);
    var statuses = new SampleStatuses(db);
    var orders = new SampleOrders(db);

    Console.WriteLine("Adding Users...");
    users.AddData();
    Console.WriteLine("Done.");

    Console.WriteLine("Adding Statuses...");
    statuses.AddData();
    Console.WriteLine("Done.");

    Console.WriteLine("Adding Orders...");
    orders.AddData();
    Console.WriteLine("Done.");

    Console.WriteLine("Adding Admin User...");
    users.AddAdmin();
    Console.WriteLine("Done.");

    users.PrintData();
    statuses.PrintData();
    orders.PrintData();
}

void DeleteAllData()
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();

    var connectionString = config.GetConnectionString("AppDbContext");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("ConnectionString is not found.");
    }

    using var connection = new MySqlConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText = "SET FOREIGN_KEY_CHECKS = 0;"
                          + "DELETE FROM Orders;"
                          + "DELETE FROM Accounts;"
                          + "DELETE FROM Users;"
                          + "DELETE FROM Statuses;"
                          + "SET FOREIGN_KEY_CHECKS = 1;";
    command.ExecuteNonQuery();
    
    connection.Close();
}
