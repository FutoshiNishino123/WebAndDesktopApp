// 実験用のコード

using Data;
using Data.Models;
using Exercise;

using var db = new AppDbContext();
var users = new Repository<User>(db);

foreach (var user in users.List())
{
    Console.WriteLine(user.Name);
}
