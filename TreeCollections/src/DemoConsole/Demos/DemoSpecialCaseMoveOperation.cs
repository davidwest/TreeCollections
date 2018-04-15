
using System.Collections.Generic;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSpecialCaseMoveOperation
    {
        public static void Start()
        {
            // In all examples, node 251 is moved but stays within its parent (246)

            var root = GetRoot();
            Display(root[246]);

            // make it the first child
            root[251].MoveToParent(246, 0);
            Display(root[246]);

            root = GetRoot();
            Display(root[246]);

            // make it the first child even if out of range
            root[251].MoveToParent(246, -100);
            Display(root[246]);

            // ----
            root = GetRoot();
            Display(root[246]);

            // make it the last child
            root[251].MoveToParent(246, 4);
            Display(root[246]);

            // ----
            root = GetRoot();
            Display(root[246]);

            // make it the last child even if out of range
            root[251].MoveToParent(246, 100);
            Display(root[246]);

            // ----
            root = GetRoot();
            Display(root[246]);

            // no change
            root[251].MoveToParent(246, 2);
            Display(root[246]);

            // ----
            root = GetRoot();
            Display(root[246]);

            root[251].MoveToParent(246, 1);
            Display(root[246]);

            // ----
            root = GetRoot();
            Display(root[246]);

            root[251].MoveToParent(246, 3);
            Display(root[246]);
        }

        private static SimpleMutableCategoryNode GetRoot()
        {
            return new CategoryTreeLookup("Source1").GetSimpleMutableCategoryTree();
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
