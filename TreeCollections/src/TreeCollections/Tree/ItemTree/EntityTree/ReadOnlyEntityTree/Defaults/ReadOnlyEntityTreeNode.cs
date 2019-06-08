using System;

namespace TreeCollections
{
    /// <summary>
    /// Default entity tree node participating in a read-only hierarchical structure.
    /// A tree can be built from the root node in a single operation, but the hierarchical structure cannot be modified after that.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class ReadOnlyEntityTreeNode<TId, TItem> : ReadOnlyEntityTreeNode<ReadOnlyEntityTreeNode<TId, TItem>, TId, TItem>
    {
        /// <summary>
        /// Root constructor
        /// </summary>
        /// <param name="definition">Entity definition that determines identity parameters</param>
        /// <param name="rootItem">Item contained by root</param>
        /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
        public ReadOnlyEntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                      TItem rootItem,
                                      ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : base(definition, rootItem, checkOptions)
        { }

        /// <summary>
        /// Root constructor
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="rootItem">Item contained by root</param>
        /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
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
