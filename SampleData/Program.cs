using Data;
using MySqlConnector;

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

    DeleteAllData();

    SamplePeople.AddData(5);
    SampleStatuses.AddData();
    SampleOrders.AddData(100);

    SamplePeople.PrintData();
    SampleStatuses.PrintData();
    SampleOrders.PrintData();
}

void DeleteAllData()
{
    using var db = new AppDbContext();
    var connectionString = db.ConnectionString;

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
