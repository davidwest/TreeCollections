using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Diagnostics.Debug;


namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSiblingRepositioning
    {
        public static void Start()
        {
            var root = new CategoryTreeLookup("Source1").GetSimpleMutableCategoryTree();

            var sw = new Stopwatch();
            sw.Start();
            Start(root);
            sw.Stop();
            Console.WriteLine($"\n*** Post data-loaded elapsed time: {sw.Elapsed} ***");
        }

        private static void Start(SimpleMutableCategoryNode root)
        {
            Display(root);

            root[251].IncrementSiblingPosition();
            Display(root);

            root[251].IncrementSiblingPosition();
            Display(root);

            root[251].IncrementSiblingPosition();
            Display(root);

            root[251].DecrementSiblingPosition();
            Display(root);

            root[251].DecrementSiblingPosition();
            Display(root);

            root[251].DecrementSiblingPosition();
            Display(root);

            root[251].DecrementSiblingPosition();
            Display(root);

            root[251].DecrementSiblingPosition();
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
