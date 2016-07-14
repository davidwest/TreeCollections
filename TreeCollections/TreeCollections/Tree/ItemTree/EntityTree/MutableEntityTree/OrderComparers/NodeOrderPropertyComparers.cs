
using System;
using System.Collections.Generic;

namespace TreeCollections
{
    internal abstract class NodeOrderPropertyComparer<TNode, TItem> : IComparer<TNode> 
        where TNode : ItemTreeNode<TNode, TItem>
    {
        private readonly Func<TItem, IComparable> _getOrderKey;
        private readonly Func<IComparable, IComparable, int> _innerCompare; 

        protected NodeOrderPropertyComparer(Func<TItem, IComparable> getOrderKey, 
                                            Func<IComparable, IComparable, int> innerCompare)
        {
            _getOrderKey = getOrderKey;
            _innerCompare = innerCompare;
        } 

        public int Compare(TNode x, TNode y)
        {
            var xVal = _getOrderKey(x.Item);
            var yVal = _getOrderKey(y.Item);

            return _innerCompare(xVal, yVal);
        }
    }

    internal sealed class NodeAscOrderComparer<TNode, TItem> : NodeOrderPropertyComparer<TNode, TItem>
        where TNode : ItemTreeNode<TNode, TItem>
    {
        public NodeAscOrderComparer(Func<TItem, IComparable> getOrderKey) 
            : base(getOrderKey, (x,y) => x.CompareTo(y))
        { }
    }

    internal sealed class NodeDescOrderComparer<TNode, TItem> : NodeOrderPropertyComparer<TNode, TItem>
        where TNode : ItemTreeNode<TNode, TItem>
    {
        public NodeDescOrderComparer(Func<TItem, IComparable> getOrderKey)
            : base(getOrderKey, (x,y) => y.CompareTo(x))
        { }
    }
}
