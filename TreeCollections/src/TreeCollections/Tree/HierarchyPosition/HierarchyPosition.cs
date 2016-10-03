
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public partial class HierarchyPosition : IComparable<HierarchyPosition>, IEquatable<HierarchyPosition>, IEnumerable<int>
    {
        public HierarchyPosition(IReadOnlyList<int> components)
        {
            Components = components;
        }

        public HierarchyPosition(params int[] components)
        {
            Components = components;
        }

        public int CompareTo(HierarchyPosition other)
        {
            int? determinant = null;

            var i = 0;
            while (!determinant.HasValue)
            {
                determinant = GetDeterminant(other, ref i);
            }

            return determinant.Value;
        }

        public IReadOnlyList<int> Components { get; }
        public int Level => Components.Count - 1;
        public int ChildOrderIndex => Components.LastOrDefault();

        private int Length => Components.Count;

        private int? GetDeterminant(HierarchyPosition other, ref int i)
        {
            if (i == Length)
            {
                return other.Length > i
                        ? -1
                        : 0;
            }

            if (i == other.Length)
            {
                return 1;
            }

            var comparison = Components[i].CompareTo(other.Components[i]);
            if (comparison != 0)
            {
                return comparison;
            }

            i++;
            return null;
        }

        public string ToString(string separator) => Components.SerializeToString(separator);


        public bool Equals(HierarchyPosition other) => Components.SequenceEqual(other.Components);


        public override bool Equals(object obj)
        {
            var hp = obj as HierarchyPosition;

            return hp == null || Equals(hp);
        }

        public override int GetHashCode() => Components.GetHashCode();


        public IEnumerator<int> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
