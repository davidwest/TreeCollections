using System.Collections.Generic;

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem> : EntityTreeNode<TNode, TId, TItem> 
        where TNode: MutableEntityTreeNode<TNode, TId, TItem>
    {
        private readonly List<TNode> _children;
        
        // --- root (private) ---
        private MutableEntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                      ErrorCheckOptions checkOptions,
                                      TItem rootItem,
                                      List<TNode> emptyChildren)
            : base(definition, checkOptions, rootItem, emptyChildren)
        {
            _children = emptyChildren;
        }

        // --- descendant (private) ---
        private MutableEntityTreeNode(TItem item, TNode parent, List<TNode> emptyChildren)
            : base(item, parent, emptyChildren)
        {
            _children = emptyChildren;
        }
        
        // --- root ---
        protected MutableEntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                        TItem rootItem, 
                                        ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : this(definition, checkOptions, rootItem, new List<TNode>())
        { }
        
        // --- descendant ---
        protected MutableEntityTreeNode(TItem item, TNode parent)
            : this(item, parent, new List<TNode>())
        { }

        public sealed override bool IsReadOnly => false;

        protected virtual void OnNodeDetaching() { }
        protected virtual void OnNodeDetached(TNode formerParent) { }
        protected virtual void OnChildrenReordered() { }
    }
}
