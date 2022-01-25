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
                    Description = "サンプルです。",
                    Version = "Ver." + Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                };
            });
        }
    }
}
