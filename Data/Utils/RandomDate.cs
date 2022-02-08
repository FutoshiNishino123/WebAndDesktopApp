using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class RandomDate
    {
        private readonly Random _random = new Random();
        private readonly DateTime _min;
        private readonly DateTime _max;

        public RandomDate(DateTime min, DateTime max)
        {
            _min = min;
            _max = max;
        }

        public DateTime Next()
        {
            var span = _max - _min;
            var rand = _random.NextDouble();
            var seconds = span.TotalSeconds * rand;
            var result = _min.AddSeconds(seconds);
            return result;
        }
    }
}
