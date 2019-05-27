using System.Collections.Generic;
using static System.Diagnostics.Debug;


namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoDefaultEntityTrees
    {
        public static void Start()
        {
            var dataRoot = new CategoryTreeLookup("Source2").GetCategoryDataTree();
            
            var mutableRoot = 
                new MutableEntityTreeNode<long, CategoryItem>(CategoryEntityDefinition<CategoryItem>.Instance,
                                                              new CategoryItem(dataRoot.CategoryId, dataRoot.Name));

            mutableRoot.Build(dataRoot, x => new CategoryItem(x.CategoryId, x.Name));
            Display(mutableRoot);

            var readOnlyRoot =
                new ReadOnlyEntityTreeNode<long, CategoryItem>(CategoryEntityDefinition<CategoryItem>.Instance,
                                                               new CategoryItem(dataRoot.CategoryId, dataRoot.Name));
            
            readOnlyRoot.Build(dataRoot, x => new CategoryItem(x.CategoryId, x.Name));
            Display(readOnlyRoot);
        }

        private static void Display(IEnumerable<MutableEntityTreeNode<long, CategoryItem>> root)
        {
            WriteLine("\n" + root.ToString(ToString));
        }

        private static void Display(IEnumerable<ReadOnlyEntityTreeNode<long, CategoryItem>> root)
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
