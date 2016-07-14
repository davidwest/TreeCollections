
using System.Collections.Generic;

namespace TreeCollections
{
    internal class NodeOrderItemComparer<TNode, TItem> : IComparer<TNode> 
        where TNode : ItemTreeNode<TNode, TItem>
    {
        private readonly IComparer<TItem> _itemComparer;

        public NodeOrderItemComparer(IComparer<TItem> itemComparer)
        {
            _itemComparer = itemComparer;
        }
        
        public int Compare(TNode x, TNode y)
        {
            return _itemComparer.Compare(x.Item, y.Item);
        }
    }
}
