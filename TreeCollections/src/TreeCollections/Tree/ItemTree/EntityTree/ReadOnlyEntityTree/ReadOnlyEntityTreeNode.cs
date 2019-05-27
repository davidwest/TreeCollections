using System.Collections.Generic;

namespace TreeCollections
{
    public abstract class ReadOnlyEntityTreeNode<TNode, TId, TItem> : EntityTreeNode<TNode, TId, TItem> 
        where TNode : ReadOnlyEntityTreeNode<TNode, TId, TItem>
    {
        // --- root ---
        protected ReadOnlyEntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                         TItem rootItem,
                                         ErrorCheckOptions checkOptions = ErrorCheckOptions.Default) 
            : base(definition, 
                   checkOptions, 
                   rootItem,
                   new List<TNode>())
        { }
                
        // --- descendant ---
        protected ReadOnlyEntityTreeNode(TItem item, TNode parent)
            : base(item, parent, new List<TNode>())
        { }

        public sealed override bool IsReadOnly => true;
    }
}
