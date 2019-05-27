using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoAdvancedMutableTree2
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source2").GetAdvancedMutableCategoryTree();

            var sw = new Stopwatch();
            sw.Start();
            Start(root);
            sw.Stop();
            Console.WriteLine($"\n*** Post data-loaded elapsed time: {sw.Elapsed} ***");
        }

        private static void Start(AdvancedMutableCategoryNode root)
        {
            Display(root);

            // --- all specified --
            //root[1554].OrderChildren(1561, 3189, 3194, 3196, 3193, 3190);

            // --- some specified --
            //root[1554].OrderChildren(3194, 3190);

            // --- some specified with 1 non-existent id ---
            root[1554].OrderChildren(3194, 2135, 3190);
            Display(root);

            root[1055].AddChild(new DualStateCategoryItem(1401, "Hail Ceasar!"));
            Display(root);

            root[1166].AddChild(new DualStateCategoryItem(120, "Shazam!"));
            Display(root);

            root[1139].MoveToPositionAfter(2190);
            Display(root);
        }

        private static void Display(IEnumerable<AdvancedMutableCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(ToString));
        }

        private static void Display(IEnumerable<SimpleMutableCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(ToString));
        }

        private static void Display(IEnumerable<ReadOnlyCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(n => ToString(n) + $"     ({n.HierarchyCount})"));
        }

        private static string ToString(AdvancedMutableCategoryNode n)
        {
            var error = n.Error.Normalize();

            var errorStr = error != IdentityError.None ? $"[** {error} **]" : "";
            var disabledStr = n.IsEnabled ? "" : "DISABLED";
            return $"{n.Id} {n.Item.Name} {disabledStr} [{n.HierarchyId.ToString("/")}] {errorStr} ({n.ContentUsageCount})";
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
