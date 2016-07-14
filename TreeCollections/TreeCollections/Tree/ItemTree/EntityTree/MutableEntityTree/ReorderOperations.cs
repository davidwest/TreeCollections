
using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        public virtual void OrderChildrenAscending<TOrderKey>(Func<TItem, TOrderKey> selectKey)
            where TOrderKey : IComparable
        {
            var comparer = new NodeAscOrderComparer<TNode, TItem>(item => selectKey(item));

            _children.Sort(comparer);
            SetChildrenSiblingReferences();
        }


        public virtual void OrderChildrenDescending<TOrderKey>(Func<TItem, TOrderKey> selectKey)
            where TOrderKey : IComparable
        {
            var comparer = new NodeDescOrderComparer<TNode, TItem>(item => selectKey(item));

            _children.Sort(comparer);
            SetChildrenSiblingReferences();
        }


        public virtual void OrderChildren(IComparer<TItem> itemComparer)
        {
            var comparer = new NodeOrderItemComparer<TNode, TItem>(itemComparer);

            _children.Sort(comparer);
            SetChildrenSiblingReferences();
        }
        

        public virtual void OrderChildren(params TId[] preferredOrder)
        {
            var comparer = 
                new NodeOrderPreferredOrderComparer<TNode, TId, TItem>(_children.Select(n => n.Id).ToArray(), 
                                                                       preferredOrder, 
                                                                       Definition.IdEqualityComparer);

            _children.Sort(comparer);
            SetChildrenSiblingReferences();
        }
    }
}
