using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public static class EntityTreeBuildExtensions
    {
        /// <summary>
        /// Build tree from parent/root node using source sequence of arbitrary type.
        /// Uses functions to retrieve parent Id and item from source objects.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="parentNode">Parent/root destination node</param>
        /// <param name="sourceItems">Sequence of source objects</param>
        /// <param name="getItem">Get item from source object</param>
        /// <param name="getParentId">Get parent Id from source object</param>
        /// <param name="maxRelativeDepth"></param>
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

        /// <summary>
        /// Build tree from parent/root node using order-able source sequence of arbitrary type.
        /// Uses functions to retrieve parent Id and item from source objects.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="parentNode">Parent/root destination node</param>
        /// <param name="sourceItems">Sequence of source objects</param>
        /// <param name="getItem">Get item from source object</param>
        /// <param name="getParentId">Get parent Id from source object</param>
        /// <param name="orderChildren">Order children</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public static void Build<TNode, TId, TItem, TSource>(this TNode parentNode,
                                                             IEnumerable<TSource> sourceItems,
                                                             Func<TSource, TItem> getItem,
                                                             Func<TSource, TId> getParentId,
                                                             Func<IEnumerable<TSource>, IEnumerable<TSource>> orderChildren, 
                                                             int? maxRelativeDepth = null)
            where TNode : EntityTreeNode<TNode, TId, TItem>
        {
            var sourceItemsGroupedByParentId = sourceItems.ToLookup(getParentId);

            parentNode.Build(p => orderChildren(sourceItemsGroupedByParentId[p.Id]).Select(getItem), maxRelativeDepth);
        }
    }
}
