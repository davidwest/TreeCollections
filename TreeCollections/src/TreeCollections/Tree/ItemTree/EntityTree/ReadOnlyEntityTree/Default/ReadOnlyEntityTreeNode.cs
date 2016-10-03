
using System;

namespace TreeCollections
{
    public class ReadOnlyEntityTreeNode<TId, TItem> : ReadOnlyEntityTreeNode<ReadOnlyEntityTreeNode<TId, TItem>, TId, TItem>
    {         
        public ReadOnlyEntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                      TItem rootItem,
                                      ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : base(definition, rootItem, checkOptions)
        { }

        public ReadOnlyEntityTreeNode(Func<TItem, TId> getId, 
                                      TItem rootItem, 
                                      ErrorCheckOptions checkOptions = ErrorCheckOptions.Default) 
            : this(new EntityDefinition<TId, TItem>(getId), rootItem, checkOptions)
        { } 

        private ReadOnlyEntityTreeNode(TItem item, ReadOnlyEntityTreeNode<TId, TItem> parent) : base(item, parent)
        { }

        protected sealed override ReadOnlyEntityTreeNode<TId, TItem> Create(TItem item, ReadOnlyEntityTreeNode<TId, TItem> parent)
        {
            return new ReadOnlyEntityTreeNode<TId, TItem>(item, parent);
        }
    }
}
