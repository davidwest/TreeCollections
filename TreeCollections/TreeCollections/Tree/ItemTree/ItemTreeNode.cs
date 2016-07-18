
using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class ItemTreeNode<TNode, TItem> : TreeNode<TNode>, IItemTreeNode<TItem>
        where TNode : ItemTreeNode<TNode, TItem>
    {
        private readonly List<TNode> _children;

        private bool _isBuilt;
        
        protected ItemTreeNode(TItem item, 
                                TNode parent, 
                                List<TNode> emptyChildren)
            : base(parent, emptyChildren)
        {
            Item = item;

            _children = emptyChildren;
        }

        public TItem Item { get; }

        public int OrderIndex => Parent?._children.IndexOf(This) ?? -1;

        protected abstract TNode Create(TItem item, TNode parent);
        
        protected virtual void OnNodeAttached() { }
        
        protected virtual void SetChildErrorsOnAttachment() { }
        protected virtual bool OnAddCanProceed() => true;
        
        internal void Build(IReadOnlyList<TItem> childItems)
        {
            if (!IsReadOnly)
            {
                InnerBuild(childItems);
                return;
            }

            if (_isBuilt)
            {
                throw new InvalidOperationException("Cannot add children to a read-only tree that has been built");
            }

            _isBuilt = true;
            InnerBuild(childItems);
        }

        private void InnerBuild(IReadOnlyCollection<TItem> childItems)
        {
            if (childItems.Count == 0 || !OnAddCanProceed())
            {
                return;
            }

            var newNodes = 
                childItems
                .Select(item => Create(item, This))
                .ToArray();

            _children.AddRange(newNodes);

            SetChildrenSiblingReferences();
            SetChildErrorsOnAttachment();

            newNodes.ForEach(n => n.OnNodeAttached());
        }
    }
}

