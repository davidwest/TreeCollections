using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public static class ItemTreeBuildExtensions
    {
        /// <summary>
        /// Build tree from parent/root node. Uses function to retrieve child items.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="parent">Parent/root destination node</param>
        /// <param name="getChildItems">Function to retrieve child items</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public static void Build<TNode, TItem>(this TNode parent, 
                                               Func<TNode, IEnumerable<TItem>> getChildItems, 
                                               int? maxRelativeDepth = null)
            where TNode : ItemTreeNode<TNode, TItem>
        {
            if (maxRelativeDepth <= 0) return;

            Build(parent, getChildItems, 0, maxRelativeDepth ?? int.MaxValue);
        }

        /// <summary>
        /// Build tree from parent/root node using arbitrary hierarchical source.
        /// Uses functions to retrieve source children and map items.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TSNode"></typeparam>
        /// <param name="parent">Parent/root destination node</param>
        /// <param name="sourceParent">Source hierarchical parent/root object</param>
        /// <param name="getChildren">Get children from source object</param>
        /// <param name="mapToItem">Map source object to item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public static void Build<TNode, TItem, TSNode>(this TNode parent, 
                                                       TSNode sourceParent, 
                                                       Func<TSNode, IReadOnlyList<TSNode>> getChildren, 
                                                       Func<TSNode, TItem> mapToItem, 
                                                       int? maxRelativeDepth = null)
            where TNode : ItemTreeNode<TNode, TItem>
        {
            if (maxRelativeDepth <= 0) return;

            Build(parent, sourceParent, getChildren, mapToItem, 0, maxRelativeDepth ?? int.MaxValue);
        }

        /// <summary>
        /// Build tree from parent/root node using arbitrary hierarchical source sequence.
        /// Uses functions to map items and retrieve hierarchy Id from source objects.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="parent">Parent/root destination node</param>
        /// <param name="sourceItems">Source hierarchical objects</param>
        /// <param name="mapToItem">Map source object to item</param>
        /// <param name="getHierarchyId">Get hierarchy Id from source object</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public static void Build<TNode, TItem, TSource>(this TNode parent, 
                                                        IEnumerable<TSource> sourceItems,
                                                        Func<TSource, TItem> mapToItem,
                                                        Func<TSource, HierarchyPosition> getHierarchyId, 
                                                        int? maxRelativeDepth = null)
            where TNode : ItemTreeNode<TNode, TItem>
        {
            if (maxRelativeDepth <= 0) return;

            var sourceItemsGroupedByParentId = sourceItems.ToLookup(getHierarchyId, HierarchyPositionParentComparer.Default);

            parent.Build(p => sourceItemsGroupedByParentId[p.HierarchyId]
                             .OrderBy(x => getHierarchyId(x).ChildOrderIndex)
                             .Select(mapToItem), maxRelativeDepth);
        }

        /// <summary>
        /// Build tree from parent/root node using source SerialTreeNode.
        /// Uses function to map source node to item. 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TSNode"></typeparam>
        /// <param name="parent">Parent/root destination node</param>
        /// <param name="sourceParent">Source SerialTreeNode parent/root</param>
        /// <param name="mapToItem">Map SerialTreeNode to item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public static void Build<TNode, TItem, TSNode>(this TNode parent, 
                                                       TSNode sourceParent, 
                                                       Func<TSNode, TItem> mapToItem, 
                                                       int? maxRelativeDepth = null)
            where TNode : ItemTreeNode<TNode, TItem>
            where TSNode : SerialTreeNode<TSNode>, new()
        {
            if (maxRelativeDepth <= 0) return;

            Build(parent, sourceParent, sp => sp.Children, mapToItem, maxRelativeDepth);
        }

        /// <summary>
        /// Build tree from parent/root node using source SerialTreeNode and filtering predicate.
        /// Uses function to map source node to item.
        /// The filtering predicate will terminate traversing source branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TSNode"></typeparam>
        /// <param name="parent">Parent/root destination node</param>
        /// <param name="sourceParent">Source SerialTreeNode parent/root</param>
        /// <param name="mapToItem">Map SerialTreeNode to item</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public static void Build<TNode, TItem, TSNode>(this TNode parent,
                                                       TSNode sourceParent,
                                                       Func<TSNode, TItem> mapToItem,
                                                       Func<TSNode, bool> allowNext,
                                                       int? maxRelativeDepth = null)
            where TNode : ItemTreeNode<TNode, TItem>
            where TSNode : SerialTreeNode<TSNode>, new()
        {
            if (!allowNext(sourceParent) || maxRelativeDepth <= 0) return;

            Build(parent, sourceParent, sp => sp.Children.Where(allowNext).ToArray(), mapToItem, maxRelativeDepth);
        }

        private static void Build<TNode, TItem>(this TNode parent, 
                                                Func<TNode, IEnumerable<TItem>> getChildItems, 
                                                int curDepth, 
                                                int maxRelativeDepth)
            where TNode : ItemTreeNode<TNode, TItem>
        {
            var childItems = getChildItems(parent).ToArray();

            parent.Build(childItems);

            if (++curDepth == maxRelativeDepth) return;

            foreach (var node in parent.Children)
            {
                Build(node, getChildItems, curDepth, maxRelativeDepth);
            }
        }

        private static void Build<TNode, TItem, TSNode>(this TNode parentNode,
                                                        TSNode sourceParent,
                                                        Func<TSNode, IReadOnlyList<TSNode>> getSourceChildItems,
                                                        Func<TSNode, TItem> mapToItem, 
                                                        int curDepth, 
                                                        int maxRelativeDepth)
            where TNode : ItemTreeNode<TNode, TItem>
        {
            var sourceChildren = getSourceChildItems(sourceParent);

            var values =
                sourceChildren
                .Select(mapToItem)
                .ToArray();

            parentNode.Build(values);

            if (++curDepth == maxRelativeDepth) return;

            var childPairs =
                parentNode.Children
                .Zip(sourceChildren, (destChild, sourceChild) => new { destChild, sourceChild });

            foreach (var pair in childPairs)
            {
                Build(pair.destChild, pair.sourceChild, getSourceChildItems, mapToItem, curDepth, maxRelativeDepth);
            }
        }
    }
}
