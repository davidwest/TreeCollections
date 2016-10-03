
using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public static class ItemTreeBuildExtensions
    {
        public static void Build<TNode, TItem>(this TNode parent, 
                                               Func<TNode, IEnumerable<TItem>> getChildItems, 
                                               int? maxRelativeDepth = null)
            where TNode : ItemTreeNode<TNode, TItem>
        {
            if (maxRelativeDepth <= 0) return;

            Build(parent, getChildItems, 0, maxRelativeDepth ?? int.MaxValue);
        }
        

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
