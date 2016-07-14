
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TreeCollections.DemoConsole.Demos
{
    public class CategoryTreeLookup
    {
        private const string PathPrefix = @"..\..\Data";
        
        private const string CategoryFileName = "categories.json";
        private const string CategoryContentMapPathFileName = "category-content-map.json";

        private readonly string _sourceName;

        public CategoryTreeLookup(string sourceName)
        {
            _sourceName = sourceName;
        }

        public SimpleMutableCategoryNode GetSimpleMutableCategoryTree()
        {
            var dataRoot = GetCategoryDataRoot();
            var root = new SimpleMutableCategoryNode(new CategoryItem(dataRoot.CategoryId, dataRoot.Name));
            root.Build(dataRoot, n => new CategoryItem(n.CategoryId, n.Name));
            return root;
        }
        
        public AdvancedMutableCategoryNode GetAdvancedMutableCategoryTree()
        {
            var dataRoot = GetCategoryDataRoot();
            var categoryContentMap = GetCategoryContentMap();

            var root = new AdvancedMutableCategoryNode(new DualStateCategoryItem(dataRoot.CategoryId, dataRoot.Name), categoryContentMap);
            root.Build(dataRoot, n => new DualStateCategoryItem(n.CategoryId, n.Name));
            return root;
        }

        public ReadOnlyCategoryNode GetReadOnlyCategoryTree()
        {
            var dataRoot = GetCategoryDataRoot();
            
            var root = new ReadOnlyCategoryNode(new DualStateCategoryItem(dataRoot.CategoryId, dataRoot.Name));
            root.Build(dataRoot, n => new DualStateCategoryItem(n.CategoryId, n.Name));
            return root;
        }

        public CategoryDataNode GetCategoryDataTree()
        {
            return GetCategoryDataRoot();
        }

        private CategoryDataNode GetCategoryDataRoot()
        {
            var path = Path.Combine(PathPrefix, _sourceName, CategoryFileName);
            var json = File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<CategoryDataNode>(json);
            return root;
        }

        private IDictionary<long, ContentItem[]> GetCategoryContentMap()
        {
            var path = Path.Combine(PathPrefix, _sourceName, CategoryContentMapPathFileName);
            var json = File.ReadAllText(path);
            var map = JsonConvert.DeserializeObject<Dictionary<long, ContentItem[]>>(json);
            return map;
        } 
    }
}
