using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    internal static class OrderNumberGenerator
    {
        private readonly static Regex Regex = new Regex(@"SN(?<number>\d+)");

        public static string? Next()
        {
            using var db = new AppDbContext();

            var max = db.Orders.Max(o => o.Number);
            if (max is null)
            {
                return null;
            }

            var match = Regex.Match(max);
            if (!match.Success)
            {
                return null;
            }

            if (!int.TryParse(match.Groups["number"].Value, out var value))
            {
                return null;
            }

            return "SN" + (value + 1).ToString("D4");
        }

        public static async Task<string?> NextAsync()
        {
            return await Task.Run(() =>
            {
                return Next();
            });
        }
    }
}
