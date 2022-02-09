using PrismApp.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrismApp.Generators
{
    internal static class OrderNumberGenerator
    {
        private readonly static Regex Regex = new Regex(@"SN(?<number>\d+)");

        public static async Task<string?> NextAsync()
        {
            var max = await OrderRepository.GetMaxNumberAsync();
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

            var next = value + 1;
            return "SN" + next.ToString("D8");
        }
    }
}
