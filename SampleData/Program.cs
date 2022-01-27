using Data;
using Data.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SampleData;

InitializeDb();

void InitializeDb()
{
    // サンプルデータを登録する前にDataプロジェクトからデータベースをマイグレーション
    // `dotnet ef database update`

    Console.Write("すべてのデータを削除して新しいデータを登録します。続行しますか？(y/n): ");
    var key = Console.ReadKey().Key;
    Console.WriteLine();
    if (key != ConsoleKey.Y)
    {
        Console.WriteLine("キャンセルされました。");
        return;
    }

    Console.WriteLine("Deleting Data...");
    DeleteAllData();
    Console.WriteLine("Done.");

    using var db = new AppDbContext();
    var users = new SampleUsers(db);
    var statuses = new SampleStatuses(db);
    var orders = new SampleOrders(db);

    Console.WriteLine("Adding Data...");
    users.AddData(5);
    statuses.AddData();
    orders.AddData(100);
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

    using var connection = new MySqlConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText = "SET FOREIGN_KEY_CHECKS = 0;"
                          + "DELETE FROM Orders;"
                          + "DELETE FROM Users;"
                          + "DELETE FROM Statuses;"
                          + "SET FOREIGN_KEY_CHECKS = 1;";
    command.ExecuteNonQuery();
    
    connection.Close();
}
