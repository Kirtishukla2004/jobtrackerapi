using Microsoft.Data.SqlClient;
using System.Reflection;

namespace JobTracker.API.Data
{
    public static class SqlHelper
    {
        private static readonly Dictionary<Type, PropertyInfo[]> _propertyCache = new();

        public static async Task<List<T>> MapToListAsync<T>(SqlDataReader reader)
            where T : new()
        {
            var result = new List<T>();

            if (!_propertyCache.TryGetValue(typeof(T), out var properties))
            {
                properties = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite)
                    .ToArray();

                _propertyCache[typeof(T)] = properties;
            }

            var columnLookup = Enumerable.Range(0, reader.FieldCount)
                .ToDictionary(reader.GetName, i => i, StringComparer.OrdinalIgnoreCase);

            while (await reader.ReadAsync())
            {
                var obj = new T();

                foreach (var prop in properties)
                {
                    if (!columnLookup.TryGetValue(prop.Name, out int index))
                        continue;

                    if (reader.IsDBNull(index))
                        continue;

                    var value = reader.GetValue(index);
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType)
                                     ?? prop.PropertyType;

                    prop.SetValue(obj, Convert.ChangeType(value, targetType));
                }

                result.Add(obj);
            }

            return result;
        }

    }
}
