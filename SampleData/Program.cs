using Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SampleData;

InitializeDb();

void InitializeDb()
{
    // サンプルデータを登録する前にDataプロジェクトからデータベースをマイグレーション
    // `dotnet ef database update`

    Console.Write("すべてのデータを削除して新しいデータを登録します。続行しますか？(y/n): ");
    if (Console.ReadKey().Key != ConsoleKey.Y)
    {
        Console.WriteLine("\nキャンセルされました。");
        return;
    }
    Console.WriteLine();

    Console.WriteLine("Deleting Data...");
    DeleteAllData();
    Console.WriteLine("Done.");

    using var db = new AppDbContext();
    var people = new SamplePeople(db);
    var statuses = new SampleStatuses(db);
    var orders = new SampleOrders(db);

    Console.WriteLine("Adding Data...");
    people.AddData(10);
    statuses.AddData();
    orders.AddData(1000);
    Console.WriteLine("Done.");

    people.PrintData();
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
                          + "DELETE FROM People;"
                          + "DELETE FROM Statuses;"
                          + "SET FOREIGN_KEY_CHECKS = 1;";
    command.ExecuteNonQuery();
    
    connection.Close();
}
