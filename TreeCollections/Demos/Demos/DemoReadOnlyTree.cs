using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoReadOnlyTree
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source2").GetReadOnlyCategoryTree();

            var sw = new Stopwatch();
            sw.Start();
            Start(root);
            sw.Stop();
            Console.WriteLine($"\n*** Post data-loaded elapsed time: {sw.Elapsed} ***");
        }

        private static void Start(ReadOnlyCategoryNode root)
        {
            Display(root);

            var copied = new ReadOnlyCategoryNode(root.Item);
            root.CompressTo(copied, n => n.Item.Name.StartsWith("M"));
            Display(copied);

            var mapCopied = new SimpleMutableCategoryNode(root.Item);
            root.MapCopyTo(mapCopied, n => n.Item as CategoryItem);
            Display(mapCopied);
        }

        private static void Display(IEnumerable<ReadOnlyCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(n => ToString(n) + $"     ({n.HierarchyCount})"));
        }

        private static void Display(IEnumerable<SimpleMutableCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(ToString));
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
