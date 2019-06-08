using System;
using System.Linq;

namespace TreeCollections
{
    /// <summary>
    /// Abstract entity tree node participating in a mutable hierarchical structure.
    /// The contained entity can have a name (alias) of any type.
    /// It can be renamed through the node, potentially resulting in an alias error being raised.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public abstract class MutableEntityTreeNode<TNode, TId, TName, TItem> : MutableEntityTreeNode<TNode, TId, TItem> 
        where TNode : MutableEntityTreeNode<TNode, TId, TName, TItem>
    {
        private readonly Func<TItem, TName> _getName;

        /// <summary>
        /// Root constructor
        /// </summary>
        /// <param name="definition">Entity definition that determines identity parameters</param>
        /// <param name="rootItem">Item contained by root</param>
        /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
        protected MutableEntityTreeNode(IEntityDefinition<TId, TName, TItem> definition, 
                                        TItem rootItem, 
                                        ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : base(definition, rootItem, checkOptions)
        {
            _getName = definition.GetName;
        }
        
        /// <summary>
        /// Descendant constructor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        protected MutableEntityTreeNode(TItem item, TNode parent) : base(item, parent)
        {
            _getName = parent._getName;
        }

        /// <summary>
        /// Quick access to item name
        /// </summary>
        public TName Name => _getName(Item);
        
        /// <summary>
        /// Rename item safely
        /// </summary>
        /// <param name="name"></param>
        public virtual void Rename(TName name)
        {
            SetItemName(name);
            
            UpdateSiblingAliasErrors();
        }
        
        protected abstract void SetItemName(TName name);

        private void UpdateSiblingAliasErrors()
        {
            var groupedByAlias = 
                Parent.Children
                .ToLookup(c => c.Item, Definition.AliasEqualityComparer);

            foreach (var grp in groupedByAlias)
            {
                if (grp.Count() > 1)
                {
                    grp.ForEach(n => n.Error |= IdentityError.SiblingAliasDuplicate);
                }
                else
                {
                    grp.ForEach(n => n.Error &= ~IdentityError.SiblingAliasDuplicate);
                }
            }
        }
    }
}
