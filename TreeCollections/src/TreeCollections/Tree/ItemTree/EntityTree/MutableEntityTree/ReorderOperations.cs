using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Arrange this node's children in ascending order based on an ordering key.
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="selectKey">Ordering key selector</param>
        public virtual void OrderChildrenAscending<TOrderKey>(Func<TItem, TOrderKey> selectKey)
            where TOrderKey : IComparable
        {
            OrderChildren(seq => seq.OrderBy(n => selectKey(n.Item)));
        }

        /// <summary>
        /// Arrange this node's children in descending order based on an ordering key.
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="selectKey">Ordering key selector</param>
        public virtual void OrderChildrenDescending<TOrderKey>(Func<TItem, TOrderKey> selectKey)
            where TOrderKey : IComparable
        {
            OrderChildren(seq => seq.OrderByDescending(n => selectKey(n.Item)));
        }

        /// <summary>
        /// Order this node's children with an item comparer
        /// </summary>
        /// <param name="itemComparer"></param>
        public virtual void OrderChildren(IComparer<TItem> itemComparer)
        {
            var comparer = new NodeOrderItemComparer<TNode, TItem>(itemComparer);

            ChildrenList.Sort(comparer);
            SetChildrenSiblingReferences();

            OnChildrenReordered();
        }

        /// <summary>
        /// Specify explicit ordering of children by Id.
        /// Id's not corresponding to children will be ignored.
        /// Not all children need to be specified. However, specified children take precedence; 
        /// unspecified children will be moved (if necessary) behind the specified ones.
        /// </summary>
        /// <param name="preferredOrder"></param>
        public virtual void OrderChildren(params TId[] preferredOrder)
        {
            var existingOrder = ChildrenList.Select(n => n.Id).ToArray();
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
            var reordered = reorder(ChildrenList).ToArray();
                
            ChildrenList.Clear();
            ChildrenList.AddRange(reordered);

            SetChildrenSiblingReferences();

            OnChildrenReordered();
        }
    }
}
