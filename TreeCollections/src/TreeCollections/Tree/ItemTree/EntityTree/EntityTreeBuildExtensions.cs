using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public static class EntityTreeBuildExtensions
    {
        public static void Build<TNode, TId, TItem, TSource>(this TNode parentNode,
                                                             IEnumerable<TSource> sourceItems,
                                                             Func<TSource, TItem> getItem,
                                                             Func<TSource, TId> getParentId, 
                                                             int? maxRelativeDepth = null)
            where TNode : EntityTreeNode<TNode, TId, TItem>
        {
            var sourceItemsGroupedByParentId = sourceItems.ToLookup(getParentId);

            parentNode.Build(p => sourceItemsGroupedByParentId[p.Id].Select(getItem), maxRelativeDepth);
        }
        
        public static void Build<TNode, TId, TItem, TSource>(this TNode parentNode,
                                                             IEnumerable<TSource> sourceItems,
                                                             Func<TSource, TItem> getItem,
                                                             Func<TSource, TId> getParentId,
                                                             Func<IEnumerable<TSource>, IEnumerable<TSource>> order, 
                                                             int? maxRelativeDepth = null)
            where TNode : EntityTreeNode<TNode, TId, TItem>
        {
            var sourceItemsGroupedByParentId = sourceItems.ToLookup(getParentId);

            parentNode.Build(p => order(sourceItemsGroupedByParentId[p.Id]).Select(getItem), maxRelativeDepth);
        }
    }
}
