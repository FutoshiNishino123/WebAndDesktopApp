﻿using Common.Extensions;
using Common.Utils;
using Data;
using Data.Models;
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

                var user = new User
                {
                    EmailAddress = "sample" + i + "@hallo.co.jp",
                    Password = PasswordUtils.GetHashValue(i.ToString()),
                    Name = $"{family.name} {first.name}",
                    Kana = $"{family.kana} {first.kana}",
                    Image = image,
                    Gender = gender,
                };

                return user;
            });

            var admin = new User
            {
                EmailAddress = "admin",
                Password = PasswordUtils.GetHashValue("admin"),
                IsAdmin = true,
                Name = "admin",
                Kana = "",
            };

            return users.Append(admin);
        }

        public void AddData(int count)
        {
            var data = CreateData(count);
            _db.AddRange(data);
            _db.SaveChanges();
        }

        public void PrintData()
        {
            var users = _db.Users.ToList();

            Console.WriteLine("--- Users ---");
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Name} ({u.Kana})");
            }
            Console.WriteLine("--- /Users ---");
        }
    }
}