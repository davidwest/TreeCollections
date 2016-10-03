
using System;

namespace TreeCollections
{
    public class MutableEntityTreeNode<TId, TItem> : MutableEntityTreeNode<MutableEntityTreeNode<TId, TItem>, TId, TItem>
    {         
        public MutableEntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                     TItem rootItem,
                                     ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : base(definition, rootItem, checkOptions)
        { }

        public MutableEntityTreeNode(Func<TItem, TId> getId, 
                                     TItem rootItem, 
                                     ErrorCheckOptions checkOptions = ErrorCheckOptions.Default) 
            : this(new EntityDefinition<TId, TItem>(getId), rootItem, checkOptions)
        { } 

        private MutableEntityTreeNode(TItem item, MutableEntityTreeNode<TId, TItem> parent) : base(item, parent)
        { }

        protected sealed override MutableEntityTreeNode<TId, TItem> Create(TItem item, MutableEntityTreeNode<TId, TItem> parent)
        {
            return new MutableEntityTreeNode<TId, TItem>(item, parent);
        }
    }
}
