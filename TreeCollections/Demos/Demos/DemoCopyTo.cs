using System.Collections.Generic;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoCopyTo
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source1").GetSimpleMutableCategoryTree();
            Display(root);

            var destRoot = new SimpleMutableCategoryNode(root.Item);
            root.CopyTo(destRoot, n => n.Name != "Database");

            Display(destRoot);
        }

        private static void Display(IEnumerable<SimpleMutableCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(ToString));
        }

        private static void Display(IEnumerable<ReadOnlyCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(n => ToString(n) + $"     ({n.HierarchyCount})"));
        }

        private static string ToString<TNode, TValue>(EntityTreeNode<TNode, long, TValue> n)
            where TNode : EntityTreeNode<TNode, long, TValue>
            where TValue : CategoryItem
        {
            if (n == null) return "<null>";

            var error = n.Error.Normalize();

            var errorStr = error != IdentityError.None ? $"[** {error} **]" : "";
            return $"{n.Id} {n.Item.Name} [{n.HierarchyId.ToString("/")}] {errorStr}";
        }
    }
}
