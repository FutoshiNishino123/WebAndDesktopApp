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
        public static async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            using var db = new AppDbContext();

            var results = await db.People.ToListAsync();
            
            return results;
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

            db.Update(person);
            
            await db.SaveChangesAsync();
        }

        public static async Task DeletePersonAsync(int id)
        {
            using var db = new AppDbContext();

            var target = id > 0 ? await db.People.FirstOrDefaultAsync(p => p.Id == id) : null;
            if (target is null)
            {
                return;
            }

            db.Remove(target);

            await db.SaveChangesAsync();
        }
    }
}
