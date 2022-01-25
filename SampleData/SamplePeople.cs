using Common.Extensions;
using Data;
using Data.Models;
using System.Text.Json;

namespace SampleData
{
    internal class SamplePeople
    {
        private readonly AppDbContext _db;

        public SamplePeople(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Person> CreateData(int count)
        {
            var obj = Data.RootobjectLoader.Load();
            if (obj == null)
            {
                return Enumerable.Empty<Person>();
            }

            var male = 0;
            var female = 0;

            var people = Enumerable.Range(0, count).Select(i =>
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

                var person = new Person
                {
                    Name = $"{family.name} {first.name}",
                    Kana = $"{family.kana} {first.kana}",
                    Image = image,
                    Gender = gender,
                };

                return person;
            });

            return people;
        }

        public void AddData(int count)
        {
            var data = CreateData(count);
            _db.AddRange(data);
            _db.SaveChanges();
        }

        public void PrintData()
        {
            var people = _db.People.ToList();

            Console.WriteLine("--- People ---");
            foreach (var p in people)
            {
                Console.WriteLine($"{p.Name} ({p.Kana})");
            }
            Console.WriteLine("--- /People ---");
        }
    }
}