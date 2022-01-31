using System.Reflection;
using System.Threading.Tasks;
using PrismApp.ViewModels;

namespace PrismApp.Models
{
    internal static class AboutRepository
    {
        public static async Task<About> GetAboutAsync()
        {
            return await Task.Run(() =>
            {
                return new About
                {
                    Description = "このアプリは 指図伝票 管理アプリ（デモ版）です。",
                    Version = "Ver." + Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                };
            });
        }
    }
}
