using Common.Extensions;
using Common.Utils;
using Data;
using Data.Models;
using System.Data.Entity;
using System.Text.Json;

namespace SampleData
{
    internal class SampleUsers
    {
        private readonly AppDbContext _db;

        public SampleUsers(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> CreateData(int count)
        {
            var obj = Data.RootobjectLoader.Load();
            if (obj == null)
            {
                return Enumerable.Empty<User>();
            }

            var male = 0;
            var female = 0;

            var users = Enumerable.Range(0, count).Select(i =>
            {
                var birth = new RandomDate(DateTime.Today.AddYears(-60), DateTime.Today.AddYears(-23));
                var family = obj.family_name.items.ElementAtRandom();
                var first = obj.first_name.items.ElementAtRandom();

                var image = first.gender switch
                {
                    "M" => obj.image.items.Where(s => s.Contains("male")).Skip(male++).FirstOrDefault(),
                    "F" => obj.image.items.Where(s => s.Contains("female")).Skip(female++).FirstOrDefault(),
                    _ => ""
                };

                var gender = first.gender switch
                {
                    "M" => Gender.Male,
                    "F" => Gender.Female,
                    "O" => Gender.Other,
                    _ => Gender.Unknown
                };

                var account = new Account
                {
                    Id = "sample" + i,
                    Password = PasswordUtils.GetHash("sample" + i),
                };

                var user = new User
                {
                    Account = account,
                    Name = $"{family.name} {first.name}",
                    Kana = $"{family.kana} {first.kana}",
                    Gender = gender,
                    BirthDate = birth.Next(),
                    Image = image,
                };

                return user;
            });

            return users;
        }

        public void AddData(int count)
        {
            var data = CreateData(count);
            _db.AddRange(data);
            _db.SaveChanges();
        }

        public void AddAdmin()
        {
            var account = new Account
            {
                Id = "admin",
                Password = PasswordUtils.GetHash("admin"),
                IsAdmin = true,
            };
            
            var user = new User
            {
                Account = account,
                Name = "admin",
                Kana = "admin",
            };

            _db.Add(user);
            _db.SaveChanges();
        }

        public void PrintData()
        {
            var users = _db.Users.Include(u => u.Account).ToList();

            Console.WriteLine("--- Users ---");
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Account?.Id}: {u.Name} ({u.Kana})");
            }
            Console.WriteLine("--- /Users ---");
        }
    }
}