using Common.Utils;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class ObjectExtensions
    {
        public static void CopyPropertiesTo(this object obj, object target)
        {
            PropertyUtils.CopyPropertiesTo(obj, target);
        }

        public static dynamic? GetProperty(this object obj, string propertyName)
        {
            return PropertyUtils.GetProperty(obj, propertyName);
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return PropertyUtils.HasProperty(obj, propertyName);
        }

        public static T Copy<T>(this object obj)
            where T : class, new()
        {
            return PropertyUtils.Copy<T>(obj);
        }
    }
}
