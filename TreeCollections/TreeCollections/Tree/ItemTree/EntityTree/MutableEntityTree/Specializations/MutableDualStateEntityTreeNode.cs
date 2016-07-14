
using System;
using System.Linq;

namespace TreeCollections
{
    public abstract class MutableDualStateEntityTreeNode<TNode, TId, TName, TItem> : MutableEntityTreeNode<TNode, TId, TName, TItem>
        where TNode : MutableDualStateEntityTreeNode<TNode, TId, TName, TItem>
    {
        private readonly Func<TItem, bool> _isEnabled;
         
        // --- root ---
        protected MutableDualStateEntityTreeNode(IDualStateEntityDefinition<TId, TName, TItem> definition,
                                                 TItem rootItem,
                                                 ErrorCheckOptions checkOptions = ErrorCheckOptions.Default)
            : base(definition, rootItem, checkOptions)
        {
            _isEnabled = definition.IsEnabled;
        }
        
        // --- descendant ---
        protected MutableDualStateEntityTreeNode(TItem item, TNode parent) : base(item, parent)
        {
            _isEnabled = parent._isEnabled;
        }

        public bool IsEnabled => _isEnabled(Item);

        public virtual void Enable()
        {
            SetState(Item, true);
            UpdateSiblingAliasErrors();
        }

        public virtual void Disable()
        {
            SetState(Item, false);
            UpdateSiblingAliasErrors();
        }
        
        protected abstract void SetState(TItem value, bool state);

        protected bool OneOrMoreInPathAreDisabled => SelectPathUpward().Any(n => !n.IsEnabled);
    }
}
