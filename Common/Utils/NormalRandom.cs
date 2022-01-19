using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class NormalRandom
    {
        /// <summary>
        /// 正規分布乱数を取得します。
        /// </summary>
        /// <param name="sigma">標準偏差</param>
        /// <param name="average">平均</param>
        /// <returns>正規分布乱数</returns>
        public static double Next(int sigma, int average)
        {
            return sigma * Next() + average;
        }

        /// <summary>
        /// 標準偏差1.0, 平均0.0の正規分布乱数を取得します。
        /// </summary>
        /// <returns>標準偏差1.0, 平均0.0の正規分布乱数</returns>
        public static double Next()
        {
            var X = Random.Shared.NextDouble();
            var Y = Random.Shared.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(X)) * Math.Cos(2.0 * Math.PI * Y);
        }
    }
}
