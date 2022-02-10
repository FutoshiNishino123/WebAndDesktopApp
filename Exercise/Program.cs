// 実験用のコード

using Data;
using Data.Models;
using Exercise;

using var db = new AppDbContext();
var users = new Repository<User>(db);
var accounts = new Repository<Account>(db);

PrintAll();


void PrintAll()
{
    foreach (var user in users.List())
    {
        Console.WriteLine(user.Name + user.Account.Name);
    }

    Console.WriteLine("done.");
}


void InsertAndDelete()
{
    var newUser = new User()
    {
        Account = new Account
        {
            Name = "test",
            Password = "test",
        },
        Name = "test",
        Kana = "test",
    };

    users.Insert(newUser);
    newUser.Kana = "abc";
    users.Update(newUser);
    users.Delete(newUser);
    accounts.Delete(newUser.Account);

    Console.WriteLine("done.");
}
