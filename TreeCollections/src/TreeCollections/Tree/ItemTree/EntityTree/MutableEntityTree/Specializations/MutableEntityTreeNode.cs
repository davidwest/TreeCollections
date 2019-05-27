using System;
using System.Linq;

namespace TreeCollections
{
    public abstract class MutableEntityTreeNode<TNode, TId, TName, TItem> : MutableEntityTreeNode<TNode, TId, TItem> 
        where TNode : MutableEntityTreeNode<TNode, TId, TName, TItem>
    {
        private readonly Func<TItem, TName> _getName; 
         
        // --- root ---
        protected MutableEntityTreeNode(IEntityDefinition<TId, TName, TItem> definition, 
                                        TItem rootItem, 
                                        ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : base(definition, rootItem, checkOptions)
        {
            _getName = definition.GetName;
        }
        
        // --- descendant ---
        protected MutableEntityTreeNode(TItem item, TNode parent) : base(item, parent)
        {
            _getName = parent._getName;
        }

        public TName Name => _getName(Item);
        
        public virtual void Rename(TName name)
        {
            SetItemName(name);
            
            UpdateSiblingAliasErrors();
        }
        
        protected abstract void SetItemName(TName name);

        protected void UpdateSiblingAliasErrors()
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
