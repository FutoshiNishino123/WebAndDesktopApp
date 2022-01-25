﻿using Common.Extensions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    public static class StatusesRepository
    {
        public static async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            using var db = new AppDbContext();

            var results = await db.Statuses.ToListAsync();
            
            return results;
        }

        public static async Task<Status?> GetStatusAsync(int id)
        {
            using var db = new AppDbContext();
            
            var result = await db.Statuses.FirstOrDefaultAsync(s => s.Id == id);
            
            return result;
        }

        public static async Task SaveStatusAsync(Status status)
        {
            using var db = new AppDbContext();

            db.Update(status);
            
            await db.SaveChangesAsync();
        }

        public static async Task DeleteStatusAsync(int id)
        {
            using var db = new AppDbContext();

            var target = await db.Statuses.FirstOrDefaultAsync(s => s.Id == id);
            if (target == null)
            {
                return;
            }

            db.Remove(target);
            
            await db.SaveChangesAsync();
        }
    }
}