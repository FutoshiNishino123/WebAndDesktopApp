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
    public class StatusesRepository
    {
        private AppDbContext _context;

        public StatusesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status?> FindAsync(int id)
        {
            return await _context.Statuses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAsync(Status status)
        {
            _context.Update(status);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var target = await _context.Statuses.FirstOrDefaultAsync(x => x.Id == id);
            if (target is null)
            {
                return;
            }

            _context.Remove(target);

            await _context.SaveChangesAsync();
        }
    }
}
