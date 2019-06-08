using System;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class ItemTreeNode<TNode, TItem>
    {
        /// <summary>
        /// Deep copy this to node of same type starting at destination parent; items will be reference copies.
        /// </summary>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void CopyTo(TNode destParent,
                           int? maxRelativeDepth = null)
        {
            MapCopyTo<TNode, TItem>(destParent, null, node => node.Item, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to node of same type starting at destination parent; uses an item mapping function.
        /// </summary>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="mapToDestItem">Map source node to destination item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void CopyTo(TNode destParent, 
                           Func<TNode, TItem> mapToDestItem, 
                           int? maxRelativeDepth = null)
        {
            MapCopyTo<TNode, TItem>(destParent, null, mapToDestItem, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to node of same type starting at destination parent; uses a filtering predicate; items will be reference copies.
        /// The filtering predicate will terminate traversing source branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void CopyTo(TNode destParent,
                           Func<TNode, bool> allowNext,
                           int? maxRelativeDepth = null)
        {
            MapCopyTo<TNode, TItem>(destParent, allowNext, node => node.Item, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to node of same type starting at destination parent; uses an item mapping function and a filtering predicate.
        /// The filtering predicate will terminate traversing source branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="mapToDestItem">Map source node to destination item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void CopyTo(TNode destParent,
                           Func<TNode, bool> allowNext,
                           Func<TNode, TItem> mapToDestItem,
                           int? maxRelativeDepth = null)
        {
            MapCopyTo<TNode, TItem>(destParent, allowNext, mapToDestItem, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to destination node type starting at destination parent; items will be reference copies.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void MapCopyTo<TDestNode>(TDestNode destParent,
                                         int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TItem>
        {
            MapCopyTo<TDestNode, TItem>(destParent, null, node => node.Item, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to destination node type starting at destination parent; uses an item mapping function.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="mapToDestItem">Map source node to destination item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void MapCopyTo<TDestNode>(TDestNode destParent,
                                         Func<TNode, TItem> mapToDestItem,
                                         int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TItem>
        {
            MapCopyTo<TDestNode, TItem>(destParent, null, mapToDestItem, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to destination node type starting at destination parent; uses a filtering predicate; items will be reference copies
        /// The filtering predicate will terminate traversing source branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void MapCopyTo<TDestNode>(TDestNode destParent,
                                         Func<TNode, bool> allowNext,
                                         int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TItem>
        {
            MapCopyTo<TDestNode, TItem>(destParent, allowNext, node => node.Item, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy this to destination node type starting at destination parent; uses an item mapping function and a filtering predicate.
        /// The filtering predicate will terminate traversing source branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="mapToDestItem">Map source node to destination item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void MapCopyTo<TDestNode>(TDestNode destParent,
                                         Func<TNode, bool> allowNext,
                                         Func<TNode, TItem> mapToDestItem,
                                         int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TItem>
        {
            MapCopyTo<TDestNode, TItem>(destParent, allowNext, mapToDestItem, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy node/item to destination node/item type starting at destination parent; uses an item mapping function.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <typeparam name="TDestItem"></typeparam>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="mapToDestItem">Map source node to destination item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void MapCopyTo<TDestNode, TDestItem>(TDestNode destParent,
                                                    Func<TNode, TDestItem> mapToDestItem,
                                                    int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TDestItem>
        {
            MapCopyTo(destParent, null, mapToDestItem, maxRelativeDepth);
        }

        /// <summary>
        /// Deep copy node/item to destination node/item type starting at destination parent; uses an item mapping function and a filtering predicate.
        /// The filtering predicate will terminate traversing source branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <typeparam name="TDestNode"></typeparam>
        /// <typeparam name="TDestItem"></typeparam>
        /// <param name="destParent">Parent/root destination node</param>
        /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
        /// <param name="mapToDestItem">Map source node to destination item</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
        public void MapCopyTo<TDestNode, TDestItem>(TDestNode destParent, 
                                                    Func<TNode, bool> allowNext, 
                                                    Func<TNode, TDestItem> mapToDestItem, 
                                                    int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TDestItem>
        {
            if (maxRelativeDepth <= 0) return;

            if (allowNext == null)
            {
                NonFilteredMapCopyTo(This, destParent, mapToDestItem, 0, maxRelativeDepth ?? int.MaxValue);        
            }
            else
            {
                if (!allowNext(This)) return;
                FilteredMapCopyTo(This, destParent, allowNext, mapToDestItem, 0, maxRelativeDepth ?? int.MaxValue);
            }   
        }

        private static void FilteredMapCopyTo<TDestNode, TDestItem>(TNode sourceParent, 
                                                                    TDestNode destParent, 
                                                                    Func<TNode, bool> allowNext,
                                                                    Func<TNode, TDestItem> mapToDestItem,  
                                                                    int curDepth, 
                                                                    int maxRelativeDepth)
            where TDestNode : ItemTreeNode<TDestNode, TDestItem>
        {
            var destValues =
                sourceParent.Children
                .Where(allowNext)
                .Select(mapToDestItem)
                .ToArray();

            destParent.Build(destValues);

            if (++curDepth == maxRelativeDepth) return;

            var childPairs =
                sourceParent.Children
                .Where(allowNext)
                .Zip(destParent.Children, (sourceChild, destChild) => new { sourceChild, destChild });

            foreach (var pair in childPairs)
            {
                FilteredMapCopyTo(pair.sourceChild, pair.destChild, allowNext, mapToDestItem, curDepth, maxRelativeDepth);
            }
        }


        private static void NonFilteredMapCopyTo<TDestNode, TDestItem>(TNode sourceParent,
                                                                       TDestNode destParent,
                                                                       Func<TNode, TDestItem> mapToDestItem,
                                                                       int curDepth,
                                                                       int maxRelativeDepth)
            where TDestNode : ItemTreeNode<TDestNode, TDestItem>
        {
            var destValues =
                sourceParent.Children
                .Select(mapToDestItem)
                .ToArray();

            destParent.Build(destValues);

            if (++curDepth == maxRelativeDepth) return;

            var childPairs =
                sourceParent.Children
                .Zip(destParent.Children, (sourceChild, destChild) => new { sourceChild, destChild });

            foreach (var pair in childPairs)
            {
                NonFilteredMapCopyTo(pair.sourceChild, pair.destChild, mapToDestItem, curDepth, maxRelativeDepth);
            }
        }
    }
}
