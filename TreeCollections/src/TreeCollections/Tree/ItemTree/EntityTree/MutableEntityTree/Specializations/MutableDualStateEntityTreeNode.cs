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
            SetItemIsEnabled(true);
            UpdateSiblingAliasErrors();
        }

        public virtual void Disable()
        {
            SetItemIsEnabled(false);
            UpdateSiblingAliasErrors();
        }
        
        protected abstract void SetItemIsEnabled(bool isEnabled);

        protected bool OneOrMoreInPathAreDisabled => SelectPathUpward().Any(n => !n.IsEnabled);
    }
}
