using System.Reflection;
using System.Threading.Tasks;
using PrismApp.ViewModels;

namespace PrismApp.Controllers
{
    internal static class AboutController
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
