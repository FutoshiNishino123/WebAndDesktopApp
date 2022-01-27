using Common.Extensions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    public static class UsersRepository
    {
        public static async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var users = db.Users.OrderBy(u => u.Id).ToList();
                return users;
            });
        }

        public static async Task<User?> FindUserAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var user = db.Users.FirstOrDefault(u => u.Id == id);
                return user;
            });
        }

        public static async Task SaveUserAsync(User user)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                if (db.Users.Any(u => u.Id != user.Id && u.EmailAddress == user.EmailAddress))
                {
                    throw new InvalidOperationException("同じメールアドレスを複数登録することはできません。");
                }
                db.Update(user);
                db.SaveChanges();
            });
        }

        public static async Task DeleteUserAsync(int id)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var user = db.Users.FirstOrDefault(u => u.Id == id);
                if (user is null)
                {
                    throw new InvalidOperationException("削除する対象が見つかりません。");
                }

                db.Remove(user);
                db.SaveChanges();
            });
        }
    }
}
