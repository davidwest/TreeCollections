using System;

namespace TreeCollections.DemoConsole.Demos
{    
    public class CategoryEntityDefinition<TValue> : EntityDefinition<long, string, TValue> 
        where TValue : CategoryItem
    {
        public static readonly CategoryEntityDefinition<TValue> Instance = new CategoryEntityDefinition<TValue>();
        
        private CategoryEntityDefinition() : base(c => c.CategoryId, c => c.Name, StringComparer.OrdinalIgnoreCase)
        { }
    }

    
    public class SimpleMutableCategoryNode : MutableEntityTreeNode<SimpleMutableCategoryNode, long, string, CategoryItem>
    {
        public SimpleMutableCategoryNode(CategoryItem item)
            : base(CategoryEntityDefinition<CategoryItem>.Instance, item, ErrorCheckOptions.All)
        { }
        
        private SimpleMutableCategoryNode(CategoryItem item, SimpleMutableCategoryNode parent) 
            : base(item, parent)
        { }

        protected override SimpleMutableCategoryNode Create(CategoryItem value, SimpleMutableCategoryNode parent)
        {
            return new SimpleMutableCategoryNode(value, parent);
        }
        
        protected override void SetItemName(string name)
        {
            Item.Name = name;
        }

        protected override bool ContinueAddOnExistingError()
        {
            Console.WriteLine($"Cancelled adding to {Id} {Item.Name.WrapDoubleQuotes()} due to error: {Error}");

            return false;
        }
    }


    public class ReadOnlyCategoryNode : ReadOnlyEntityTreeNode<ReadOnlyCategoryNode, long, DualStateCategoryItem>
    {
        public ReadOnlyCategoryNode(DualStateCategoryItem item)
            : base(CategoryEntityDefinition<DualStateCategoryItem>.Instance, item, ErrorCheckOptions.All)
        {
            HierarchyCount = 1;
        }

        private ReadOnlyCategoryNode(DualStateCategoryItem item, ReadOnlyCategoryNode parent)
            : base(item, parent)
        { }

        protected override ReadOnlyCategoryNode Create(DualStateCategoryItem value, ReadOnlyCategoryNode parent)
        {
            return new ReadOnlyCategoryNode(value, parent);
        }

        public int HierarchyCount { get; private set; }

        protected override void OnNodeAttached()
        {
            SelectPathUpward().ForEach(n => n.HierarchyCount++);
        }
        
    }
}
