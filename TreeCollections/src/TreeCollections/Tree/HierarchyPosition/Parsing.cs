using System;
using System.Collections.Generic;

namespace TreeCollections
{
    public partial class HierarchyPosition
    {
        /// <summary>
        /// Parses a string to a HierarchyPosition
        /// </summary>
        /// <param name="source">String to parse</param>
        /// <param name="separators">Separators between integers in string</param>
        /// <returns></returns>
        public static HierarchyPosition TryParse(string source, params string[] separators)
        {
            var parts = source.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var values = new List<int>();

            foreach (var part in parts)
            {
                int value;
                if (!int.TryParse(part, out value))
                {
                    return null;
                }

                values.Add(value);
            }

            return new HierarchyPosition(values);
        }
    }
}
