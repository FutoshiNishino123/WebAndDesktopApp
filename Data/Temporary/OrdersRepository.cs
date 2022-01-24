using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class OrdersRepository
    {
        private AppDbContext _context;

        public OrdersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .ToListAsync();
        }

        public async Task<Order?> FindAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAsync(Order order)
        {
            // NOTE: 自動更新されないバグの対応
            order.UpdatedDate = DateTime.Now;

            _context.Update(order);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var target = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (target is null)
            {
                return;
            }

            _context.Remove(target);

            await _context.SaveChangesAsync();
        }
    }
}
