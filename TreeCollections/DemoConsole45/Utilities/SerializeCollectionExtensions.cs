
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections.DemoConsole
{
    internal static class SerializeCollectionExtensions
    {
        public static string SerializeToString<T>(this IEnumerable<T> values)
        {
            return values.SerializeToString(string.Empty, string.Empty, string.Empty);
        }

        public static string SerializeToString<T>(this IEnumerable<T> values,
                                                  char separator,
                                                  string wrapperStart = "",
                                                  string wrapperEnd = "")
        {
            return values.SerializeToString(separator.ToString(), wrapperStart, wrapperEnd);
        }

        public static string SerializeToString<T>(this IEnumerable<T> values,
                                                  string separator,
                                                  string wrapperStart = "",
                                                  string wrapperEnd = "")
        {
            var list = values.ToList();

            if (list.Count == 0)
            {
                return string.Empty;
            }

            var result = list.Select(val => $"{wrapperStart}{val}{wrapperEnd}")
                             .Aggregate((a, b) => $"{a}{separator}{b}");

            return result;
        }


        public static string ToCsv<T>(this IEnumerable<T> values)
        {
            return SerializeToString(values, ", ");
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> values)
        {
            return SerializeToString(values, ", ", "'", "'");
        }

        public static string ToDoubleQuotedCsv<T>(this IEnumerable<T> values)
        {
            return SerializeToString(values, ", ", "\"", "\"");
        }
    }
}
