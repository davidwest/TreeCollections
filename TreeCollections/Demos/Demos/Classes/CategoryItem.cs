namespace TreeCollections.DemoConsole.Demos
{
    public class CategoryItem
    {
        public CategoryItem(long categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public long CategoryId { get; }
        public string Name { get; set; }
    }
    
    public class DualStateCategoryItem : CategoryItem
    {
        public DualStateCategoryItem(long categoryId, string name)
            : base(categoryId, name)
        {
            IsEnabled = true;
        }

        public DualStateCategoryItem(CategoryItem sourceItem)
            : base(sourceItem.CategoryId, sourceItem.Name)
        {
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
    }
}
