

using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    internal class NodeOrderPreferredOrderComparer<TNode, TId, TItem> : IComparer<TNode> where TNode 
        : MutableEntityTreeNode<TNode, TId, TItem>
    {
        private readonly Dictionary<TId, int> _orderMap;
         
        public NodeOrderPreferredOrderComparer(IReadOnlyList<TId> existingOrder,
                                               IReadOnlyList<TId> preferredOrder, 
                                               IEqualityComparer<TId> comparer)
        {
            var specifiedIds = preferredOrder.Intersect(existingOrder, comparer);
            var unspecifiedIds = existingOrder.Except(preferredOrder, comparer);
            
            _orderMap =
                specifiedIds.Concat(unspecifiedIds)
                .Select((id, i) => new { id, i })
                .ToDictionary(pair => pair.id, pair => pair.i);
        } 

        public int Compare(TNode x, TNode y)
        {
            var xIndex = _orderMap.ContainsKey(x.Id) ? _orderMap[x.Id] : int.MaxValue;
            var yIndex = _orderMap.ContainsKey(y.Id) ? _orderMap[y.Id] : int.MaxValue;
            
            return xIndex.CompareTo(yIndex);
        }
    }
}
