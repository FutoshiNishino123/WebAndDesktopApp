using Data;
using Data.Models;

namespace SampleData
{
    internal static class SamplePeople
    {
        public static IEnumerable<Person> CreateData(int count)
        {
            var obj = Data.RootobjectLoader.Load();
            for (var i = 0; i < count; i++)
            {
                var family = obj?.family_name.items.ElementAtRandom();
                var first = obj?.first_name.items.ElementAtRandom();
                var person = new Person
                {
                    Id = i + 1,
                    Name = $"{family?.name} {first?.name}",
                    Kana = $"{family?.kana} {first?.kana}",
                    ImageUrl = first?.image,
                    Gender = first?.gender switch
                    {
                        "M" => Gender.Male,
                        "F" => Gender.Female,
                        "O" => Gender.Other,
                        _ => Gender.Unknown
                    },
                };
                yield return person;
            }
        }

        public static void AddData(int count)
        {
            var people = CreateData(count);
            using var db = new AppDbContext();
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