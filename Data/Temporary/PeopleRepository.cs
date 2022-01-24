using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PeopleRepository
    {
        private AppDbContext _context;

        public PeopleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person?> FindAsync(int id)
        {
            return await _context.People.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAsync(Person person)
        {
            _context.Update(person);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var target = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (target is null)
            {
                return;
            }

            _context.Remove(target);

            await _context.SaveChangesAsync();
        }
    }
}
