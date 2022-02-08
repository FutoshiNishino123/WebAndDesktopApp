using Common.Utils;

namespace Data.Extensions
{
    public static class ObjectExtensions
    {
        public static void CopyPropertiesTo(this object obj, object target)
        {
            PropertyUtils.CopyProperties(obj, target);
        }

        public static dynamic? GetProperty(this object obj, string propertyName)
        {
            return PropertyUtils.GetProperty(obj, propertyName);
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return PropertyUtils.HasProperty(obj, propertyName);
        }

        public static T Map<T>(this object obj)
            where T : class, new()
        {
            return PropertyUtils.Map<T>(obj);
        }
    }
}
