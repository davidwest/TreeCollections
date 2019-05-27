using System.Collections.Generic;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSpecialCaseMoveOperation
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source1").GetSimpleMutableCategoryTree();
            Display(root);

            root[327].MoveToParent(325);
            Display(root);

            // should be exactly the same position
            root[326].MoveToParent(325, 0);
            Display(root);

            root[326].MoveToParent(325, 1);
            Display(root);

            root[327].MoveToParent(325, 2);
            Display(root);
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
