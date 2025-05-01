using Newtonsoft.Json;

namespace ERP_Components.Helper
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));

        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void RemoveObject(this ISession session, string key)
        {
            session.Remove(key);
        }

        public static void SetGuid(this ISession session, string key, Guid value)
        {
            session.SetString(key, value.ToString());
        }

        public static Guid? GetGuid(this ISession session, string key)
        {
            var guidString = session.GetString(key);
            if (!string.IsNullOrEmpty(guidString) && Guid.TryParse(guidString, out Guid result))
            {
                return result;
            }
            return null;
        }

        public static void removeGuid(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
