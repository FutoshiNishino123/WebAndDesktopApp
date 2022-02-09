using System.Reflection;
using System.Threading.Tasks;
using PrismApp.Models;

namespace PrismApp.Data
{
    internal static class AboutRepository
    {
        public static async Task<About> GetAboutAsync()
        {
            return await Task.Run(() =>
            {
                return new About
                {
                    Description = "これは 指図伝票 管理アプリ（デモ版）です。",
                    Version = "Ver." + Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                };
            });
        }
    }
}
