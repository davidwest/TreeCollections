using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Diagnostics.Debug;


namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoAdvancedMutableTree
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source1").GetAdvancedMutableCategoryTree();

            var sw = new Stopwatch();
            sw.Start();
            Start(root);
            sw.Stop();
            Console.WriteLine($"\n*** Post data-loaded elapsed time: {sw.Elapsed} ***");
        }

        private static void Start(AdvancedMutableCategoryNode root)
        {
            Display(root);

            root[247].MoveToParent(326);
            Display(root);

            root[153].AddAtPositionAfter(new DualStateCategoryItem(new CategoryItem(2351, "uTiLiTies")));
            Display(root);

            root[325].MoveToParent(195);
            Display(root);

            root[192].MoveToParent(247);
            Display(root);

            root[153].Disable();
            Display(root);
            Display(root.PreOrder(n => n.IsEnabled));

            root[58].AddChild(new DualStateCategoryItem(3, "Wham Bam"));
            Display(root);

            root[334].AddChild(new DualStateCategoryItem(1000, "Uh Oh"));
            Display(root);

            root[334].MoveToPositionAfter(174);
            Display(root);

            root[2000].Detach();
            Display(root);

            root[153].Enable();
            Display(root);

            root[326].MoveToPositionBefore(3);
            Display(root);

            var destRoot = root.CloneAsRoot();
            root.CompressTo(destRoot, n => n.Level > 3);
            Display(destRoot);

            var mappedDestRoot = new ReadOnlyCategoryNode(root.Item);
            root.MapCompressTo(mappedDestRoot, n => n.Name.StartsWith("B"));
            Display(mappedDestRoot);
            
        }

        private static void Display(IEnumerable<AdvancedMutableCategoryNode> root)
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
            var error = n.Error.Normalize();

            var errorStr = error != IdentityError.None ? $"[** {error} **]" : "";
            return $"{n.Id} {n.Item.Name} [{n.HierarchyId.ToString("/")}] {errorStr}";
        }
    }
}
