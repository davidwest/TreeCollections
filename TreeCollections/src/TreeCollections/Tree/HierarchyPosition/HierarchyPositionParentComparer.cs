using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public class HierarchyPositionParentComparer : IEqualityComparer<HierarchyPosition>
    {
        public static readonly HierarchyPositionParentComparer Default = new HierarchyPositionParentComparer();

        public bool Equals(HierarchyPosition x, HierarchyPosition y)
        {
            return x.Take(x.Level).SequenceEqual(y.Take(y.Level));
        }

        public int GetHashCode(HierarchyPosition obj)
        {
            return obj.GetHashCode();
        }
    }
}
