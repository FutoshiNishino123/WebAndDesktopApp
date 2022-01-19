using Common.Extensions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Controllers
{
    public static class PersonController
    {
        private static readonly Func<Person, bool> NoFilter = _ => true;

        public static async Task<IEnumerable<Person>> GetPeopleAsync(Func<Person, bool>? filter = null)
        {
            using var db = new AppDbContext();
            var results = await db.People.ToListAsync();
            return results.Where(filter ?? NoFilter);
        }

        public static async Task<Person?> GetPersonAsync(int id)
        {
            using var db = new AppDbContext();
            var result = await db.People.FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public static async Task SavePersonAsync(Person person)
        {
            using var db = new AppDbContext();

            Person? target = null;
            if (person.Id > 0)
            {
                target = await db.People.FirstOrDefaultAsync(p => p.Id == person.Id);
            }

            if (target is null)
            {
                target = new Person();
                db.Add(target);
            }
            
            person.CopyPropertiesTo(target);
            
            await db.SaveChangesAsync();
        }

        public static async Task DeletePersonAsync(int id)
        {
            using var db = new AppDbContext();

            Person? target = null;
            if (id > 0)
            {
                target = await db.People.FirstOrDefaultAsync(p => p.Id == id);
            }

            if (target is null)
            {
                throw new InvalidOperationException("レコードが見つかりません。");
            }
            
            db.Remove(target);
            
            await db.SaveChangesAsync();
        }
    }
}
