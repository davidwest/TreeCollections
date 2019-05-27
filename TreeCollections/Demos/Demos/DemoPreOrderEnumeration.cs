using System.Collections.Generic;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoPreOrderEnumeration
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source1").GetSimpleMutableCategoryTree();

            Display(root);
            Display(root.PreOrder(n => n.Name != "ESRI"));
        }

        private static void Display(IEnumerable<SimpleMutableCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(ToString));
        }

        private static string ToString<TNode, TValue>(EntityTreeNode<TNode, long, TValue> n)
            where TNode : EntityTreeNode<TNode, long, TValue>
            where TValue : CategoryItem
        {
            var error = n.Error.Normalize();

            var errorStr = error != IdentityError.None ? $"[** {error} **]" : "";
            return $"{n.Id} {n.Item.Name} [{n.HierarchyId.ToString("/")}] {errorStr}";
        }
    }
}
