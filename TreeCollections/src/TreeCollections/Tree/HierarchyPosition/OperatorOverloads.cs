namespace TreeCollections
{
    public partial class HierarchyPosition
    {
        public static bool operator ==(HierarchyPosition hp1, HierarchyPosition hp2)
        {
            if (ReferenceEquals(hp1, hp2))
            {
                return true;
            }

            if (((object)hp1 == null) || ((object)hp2 == null))
            {
                return false;
            }

            return hp1.Equals(hp2);
        }


        public static bool operator !=(HierarchyPosition hp1, HierarchyPosition hp2)
        {
            return !(hp1 == hp2);
        }


        public static bool operator <(HierarchyPosition hp1, HierarchyPosition hp2)
        {
            if (!CanCompare(hp1, hp2)) return false;

            return hp1.CompareTo(hp2) < 0;
        }


        public static bool operator >(HierarchyPosition hp1, HierarchyPosition hp2)
        {
            if (!CanCompare(hp1, hp2)) return false;

            return hp1.CompareTo(hp2) > 0;
        }


        private static bool CanCompare(HierarchyPosition hp1, HierarchyPosition hp2)
        {
            if (ReferenceEquals(hp1, hp2))
            {
                return false;
            }

            if (((object)hp1 == null) || ((object)hp2 == null))
            {
                return false;
            }

            return true;
        }
    }
}
