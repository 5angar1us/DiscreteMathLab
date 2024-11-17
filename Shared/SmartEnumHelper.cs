using Ardalis.SmartEnum;
using System.Reflection;

namespace Shared;

public static class SmartEnumHelper {
    public static List<T> SortSmartEnumByValue<T>() where T : SmartEnum<T> {
        var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                              .Where(f => f.FieldType == typeof(T));

        return fields.Select(f => (T)f.GetValue(null))
                     .OrderBy(x => x.Value)
                     .ToList();
    }
}
