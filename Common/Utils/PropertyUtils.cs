using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Common.Utils
{
    public static class PropertyUtils
    {
        /// <summary>
        /// 同名かつ同型のPublicプロパティをコピーします。
        /// </summary>
        /// <param name="source">コピー元のオブジェクト</param>
        /// <param name="target">コピー先のオブジェクト</param>
        public static void CopyPropertiesTo(object source, object target)
        {
            if (source == null || target == null)
            {
                return;
            }

            // コピー元のPublicプロパティ（読み書き可）を取得
            var props1 = source.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(p => p.CanRead && p.CanWrite);

            // コピー先のPublicプロパティ（読み書き可）を取得
            var props2 = target.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(p => p.CanRead && p.CanWrite);

            //  同名かつ同型のプロパティでペアを作成
            var pairs = props1.Join(props2, p1 => p1.Name, p2 => p2.Name, (p1, p2) => new { p1, p2 })
                              .Where(x => x.p1.PropertyType == x.p2.PropertyType);

            // プロパティをコピー
            foreach (var p in pairs)
            {
                p.p2.SetValue(target, p.p1.GetValue(source));
            }
        }

        /// <summary>
        /// 同名かつ同型のPublicプロパティをコピーしたクローンを作成します。
        /// </summary>
        /// <typeparam name="T">クローンオブジェクトの型</typeparam>
        /// <param name="obj">コピー元オブジェクト</param>
        /// <returns>クローンオブジェクト</returns>
        public static T Copy<T>(object obj)
            where T : class, new()
        {
            var result = new T();
            CopyPropertiesTo(obj, result);
            return result;
        }

        /// <summary>
        /// プロパティを取得します。
        /// </summary>
        /// <param name="obj">プロパティを取得するオブジェクト</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>取得したプロパティ</returns>
        public static dynamic? GetProperty(object obj, string propertyName)
        {
            if (obj is ExpandoObject)
            {
                return ((IDictionary<string, object>)obj)[propertyName];
            }

            return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
        }

        /// <summary>
        /// プロパティが存在するか判定します。
        /// </summary>
        /// <param name="obj">プロパティを判定するオブジェクト</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティが存在すればtrue</returns>
        public static bool HasProperty(object obj, string propertyName)
        {
            if (obj is ExpandoObject)
            {
                return ((IDictionary<string, object>)obj).ContainsKey(propertyName);
            }

            return obj.GetType().GetProperty(propertyName) != null;
        }
    }
}
