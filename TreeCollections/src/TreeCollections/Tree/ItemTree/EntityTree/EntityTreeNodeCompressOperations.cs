using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class EntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Build compressed version of a tree with the same node type based on filtering criteria.
        /// Nodes will be copied to the new tree if they match the criteria OR are ancestors to nodes that do.
        /// Items in the new tree are reference copies of those in the source tree.
        /// </summary>
        /// <param name="destRoot">Parent/root destination node</param>
        /// <param name="matchesCriteria">Criteria predicate</param>
        /// <param name="maxRelativeSearchDepth">Max depth of traversal when finding matching nodes in source</param>
        /// <param name="maxRelativeRenderDepth">Max depth to build compressed version</param>
        public void CompressTo(TNode destRoot,
                               Func<TNode, bool> matchesCriteria,
                               int? maxRelativeSearchDepth = null,
                               int? maxRelativeRenderDepth = null)
        {
            MapCompressTo(destRoot, matchesCriteria, node => node.Item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }

        /// <summary>
        /// Build compressed version of a tree with the same node type based on filtering criteria.
        /// Nodes will be copied to the new tree if they match the criteria OR are ancestors to nodes that do.
        /// Items in the new tree are reference copies of those in the source tree.
        /// </summary>
        /// <param name="destRoot">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="matchesCriteria">Main criteria predicate</param>
        /// <param name="maxRelativeSearchDepth">Max depth of traversal when finding matching nodes in source</param>
        /// <param name="maxRelativeRenderDepth">Max depth to build compressed version</param>
        public void CompressTo(TNode destRoot,
                               Func<TNode, bool> allowNext,
                               Func<TNode, bool> matchesCriteria,
                               int? maxRelativeSearchDepth = null,
                               int? maxRelativeRenderDepth = null)
        {
            MapCompressTo(destRoot, allowNext, matchesCriteria, node => node.Item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }

        /// <summary>
        /// Build compressed version of a tree with destination node type based on filtering criteria.
        /// Nodes will be copied to the new tree if they match the criteria OR are ancestors to nodes that do.
        /// Items in the new tree are reference copies of those in the source tree.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <param name="destRoot">Parent/root destination node</param>
        /// <param name="matchesCriteria">Criteria predicate</param>
        /// <param name="maxRelativeSearchDepth">Max depth of traversal when finding matching nodes in source</param>
        /// <param name="maxRelativeRenderDepth">Max depth to build compressed version</param>
        public void MapCompressTo<TDestNode>(TDestNode destRoot,
                                             Func<TNode, bool> matchesCriteria,
                                             int? maxRelativeSearchDepth = null,
                                             int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TItem>
        {
            MapCompressTo(destRoot, matchesCriteria, node => node.Item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }

        /// <summary>
        /// Build compressed version of a tree with destination node type based on filtering criteria.
        /// Nodes will be copied to the new tree if they match the criteria OR are ancestors to nodes that do.
        /// Items in the new tree are reference copies of those in the source tree.
        /// </summary>
        /// <param name="destRoot">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="matchesCriteria">Main criteria predicate</param>
        /// <param name="maxRelativeSearchDepth">Max depth of traversal when finding matching nodes in source</param>
        /// <param name="maxRelativeRenderDepth">Max depth to build compressed version</param>
        public void MapCompressTo<TDestNode>(TDestNode destRoot,
                                             Func<TNode, bool> allowNext,
                                             Func<TNode, bool> matchesCriteria,
                                             int? maxRelativeSearchDepth = null,
                                             int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TItem>
        {
            MapCompressTo(destRoot, allowNext, matchesCriteria, node => node.Item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }

        /// <summary>
        /// Build compressed version of a tree with destination node and item types based on filtering criteria.
        /// Nodes will be copied to the new tree if they match the criteria OR are ancestors to nodes that do.
        /// </summary>
        /// <param name="destRoot">Parent/root destination node</param>
        /// <param name="matchesCriteria">Criteria predicate</param>
        /// <param name="mapItem">Map source node to destination item</param>
        /// <param name="maxRelativeSearchDepth">Max depth of traversal when finding matching nodes in source</param>
        /// <param name="maxRelativeRenderDepth">Max depth to build compressed version</param>
        public void MapCompressTo<TDestNode, TDestItem>(TDestNode destRoot,
                                                        Func<TNode, bool> matchesCriteria,
                                                        Func<TNode, TDestItem> mapItem,
                                                        int? maxRelativeSearchDepth = null,
                                                        int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TDestItem>
        {
            if (maxRelativeRenderDepth <= 0) return;

            MapCompressTo(PreOrder(maxRelativeSearchDepth), destRoot, matchesCriteria, mapItem, maxRelativeRenderDepth);
        }

        /// <summary>
        /// Build compressed version of a tree with destination node and item types based on filtering criteria.
        /// Nodes will be copied to the new tree if they match the criteria OR are ancestors to nodes that do.
        /// </summary>
        /// <param name="destRoot">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="matchesCriteria">Main criteria predicate</param>
        /// <param name="mapItem">Map source node to destination item</param>
        /// <param name="maxRelativeSearchDepth">Max depth of traversal when finding matching nodes in source</param>
        /// <param name="maxRelativeRenderDepth">Max depth to build compressed version</param>
        public void MapCompressTo<TDestNode, TDestItem>(TDestNode destRoot,
                                                        Func<TNode, bool> allowNext,
                                                        Func<TNode, bool> matchesCriteria,
                                                        Func<TNode, TDestItem> mapItem,
                                                        int? maxRelativeSearchDepth = null,
                                                        int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TDestItem>
        {
            if (!allowNext(This) || maxRelativeRenderDepth <= 0) return;

            MapCompressTo(PreOrder(allowNext, maxRelativeSearchDepth), destRoot, matchesCriteria, mapItem, maxRelativeRenderDepth);
        }


        private static void MapCompressTo<TDestNode, TDestItem>(IEnumerable<TNode> sourceNodes,
                                                                TDestNode destRoot,
                                                                Func<TNode, bool> matchesCriteria,
                                                                Func<TNode, TDestItem> mapItem,
                                                                int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TDestItem>
        {
            var sourceDescendantNodePool =
                sourceNodes
                .WhereSupports(matchesCriteria)
                .Where(n => !n.IsRoot)
                .ToLookup(n => n.Parent.Id);

            if (sourceDescendantNodePool.Count == 0)
            {
                return;
            }

            destRoot.Build(parent => sourceDescendantNodePool[parent.Id].Select(mapItem), maxRelativeRenderDepth);
        }
    }
}
