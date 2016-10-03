
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
            OrderChildren(seq => seq.OrderBy(n => selectKey(n.Item)));
        }


        public virtual void OrderChildrenDescending<TOrderKey>(Func<TItem, TOrderKey> selectKey)
            where TOrderKey : IComparable
        {
            OrderChildren(seq => seq.OrderByDescending(n => selectKey(n.Item)));
        }


        public virtual void OrderChildren(IComparer<TItem> itemComparer)
        {
            var comparer = new NodeOrderItemComparer<TNode, TItem>(itemComparer);

            _children.Sort(comparer);
            SetChildrenSiblingReferences();

            OnChildrenReordered();
        }
        

        public virtual void OrderChildren(params TId[] preferredOrder)
        {
            var existingOrder = _children.Select(n => n.Id).ToArray();
            var specifiedIds = preferredOrder.Intersect(existingOrder, Definition.IdEqualityComparer);
            var unspecifiedIds = existingOrder.Except(preferredOrder, Definition.IdEqualityComparer);

            var orderMap = 
                specifiedIds.Concat(unspecifiedIds)
                .Select((id, i) => new {id, i})
                .ToDictionary(pair => pair.id, pair => pair.i, Definition.IdEqualityComparer);

            OrderChildren(seq => seq.OrderBy(n => orderMap[n.Id]));
        }


        private void OrderChildren(Func<IEnumerable<TNode>, IEnumerable<TNode>> reorder)
        {
            var reordered = reorder(_children).ToArray();
                
            _children.Clear();
            _children.AddRange(reordered);

            SetChildrenSiblingReferences();

            OnChildrenReordered();
        }
    }
}
