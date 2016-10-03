
using System;
using System.Collections.Generic;

namespace TreeCollections.DemoConsole.Demos
{
    public class CategoryDualStateEntityDefinition : DualStateEntityDefinition<long, string, DualStateCategoryItem>
    {
        public CategoryDualStateEntityDefinition() 
            : base(item => item.CategoryId, item => item.Name, item => item.IsEnabled, StringComparer.OrdinalIgnoreCase)
        { }
    }

    public class ContentItem
    {
        public ContentItem(long contentId, string name)
        {
            ContentId = contentId;
            Name = name;
        }

        public long ContentId { get; }
        public string Name { get; }
    }
    
    public class AdvancedMutableCategoryNode : MutableDualStateEntityTreeNode<AdvancedMutableCategoryNode, long, string, DualStateCategoryItem>
    {
        private readonly IDictionary<long, ContentItem[]> _categoryContentMap;

        public AdvancedMutableCategoryNode(DualStateCategoryItem rootItem, IDictionary<long, ContentItem[]> categoryContentMap)
            : base(new CategoryDualStateEntityDefinition(), rootItem, ErrorCheckOptions.All)
        {
            _categoryContentMap = categoryContentMap;
        }

        public AdvancedMutableCategoryNode CloneAsRoot()
        {
            return new AdvancedMutableCategoryNode(Item, _categoryContentMap);
        }

        private AdvancedMutableCategoryNode(DualStateCategoryItem item, AdvancedMutableCategoryNode parent)
            : base(item, parent)
        {
            _categoryContentMap = parent._categoryContentMap;
        }

        public int ContentUsageCount { get; private set; }

        public override void Enable()
        {
            if (IsEnabled) return;
            base.Enable();
            OnNodeAttached();
        }

        public override void Disable()
        {
            if (!IsEnabled) return;
            base.Disable();
            OnNodeDetached(Parent);
        }

        public override void Rename(string name)
        {
            if (OneOrMoreInPathAreDisabled)
            {
                throw new InvalidOperationException("Cannot rename disabled item");
            }

            base.Rename(name);
        }

        protected override AdvancedMutableCategoryNode Create(DualStateCategoryItem item, AdvancedMutableCategoryNode parent)
        {
            return new AdvancedMutableCategoryNode(item, parent);
        }

        protected override void SetItemName(string name) => Item.Name = name;
        protected override void SetItemIsEnabled(bool isEnabled) => Item.IsEnabled = isEnabled;

        protected override void OnNodeAttached()
        {
            if (OneOrMoreInPathAreDisabled)
            {
                throw new InvalidOperationException("Cannot attach disabled item or to a path with a disabled ancestor");
            }
            
            if (ContentUsageCount == 0)
            {
                ContentItem[] items;
                if (!_categoryContentMap.TryGetValue(Id, out items)) return;
                ContentUsageCount = items.Length;
            }
            
            SelectAncestorsUpward().ForEach(n => n.ContentUsageCount += ContentUsageCount);
        }

        protected override void OnNodeDetached(AdvancedMutableCategoryNode formerParent)
        {
            if (formerParent.OneOrMoreInPathAreDisabled)
            {
                throw new InvalidOperationException("Cannot detach from a path with a disabled ancestor");
            }

            formerParent.SelectPathUpward().ForEach(n => n.ContentUsageCount -= ContentUsageCount);
        }
    }
}
