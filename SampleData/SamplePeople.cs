using Common.Extensions;
using Data;
using Data.Models;

namespace SampleData
{
    internal static class SamplePeople
    {
        public static IEnumerable<Person> CreateData(int count)
        {
            var obj = Data.RootobjectLoader.Load();
            var male = 0;
            var female = 0;

            for (var i = 0; i < count; i++)
            {
                var family = obj?.family_name.items.ElementAtRandom();
                var first = obj?.first_name.items.ElementAtRandom();
                var image = first?.gender switch
                {
                    "M" => obj?.image.items.Where(s => s.Contains("male")).Skip(male++).FirstOrDefault(),
                    "F" => obj?.image.items.Where(s => s.Contains("female")).Skip(female++).FirstOrDefault(),
                    _ => ""
                };
                var gender = first?.gender switch
                {
                    "M" => Gender.Male,
                    "F" => Gender.Female,
                    "O" => Gender.Other,
                    _ => Gender.Unknown
                };

                var person = new Person
                {
                    Id = i + 1,
                    Name = $"{family?.name} {first?.name}",
                    Kana = $"{family?.kana} {first?.kana}",
                    ImageUrl = image,
                    Gender = gender,
                };
                
                yield return person;
            }
        }

        public static void AddData(int count)
        {
            using var db = new AppDbContext();
            
            var people = CreateData(count);
            db.AddRange(people);
            
            db.SaveChanges();
        }

        public static void PrintData()
        {
            using var db = new AppDbContext();

            var people = db.People.ToList();
            
            Console.WriteLine("--- People ---");
            foreach (var x in people)
            {
                Console.WriteLine($"{x.Name} ({x.Kana})");
            }
            Console.WriteLine("--- /People ---");
        }
    }
}