using System;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class ItemTreeNode<TNode, TItem>
    {
        public void CopyTo(TNode destParent,
                           int? maxRelativeDepth = null)
        {
            MapCopyTo(destParent, null, node => node.Item, maxRelativeDepth);
        }


        public void CopyTo(TNode destParent,
                           Func<TNode, bool> allowNext,
                           int? maxRelativeDepth = null)
        {
            MapCopyTo(destParent, allowNext, node => node.Item, maxRelativeDepth);
        }


        public void MapCopyTo<TDestNode>(TDestNode destParent,
                                         int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TItem>
        {
            MapCopyTo(destParent, null, node => node.Item, maxRelativeDepth);
        }


        public void MapCopyTo<TDestNode>(TDestNode destParent,
                                         Func<TNode, bool> allowNext,
                                         int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TItem>
        {
            MapCopyTo(destParent, allowNext, node => node.Item, maxRelativeDepth);
        }


        public void MapCopyTo<TDestNode, TDestItem>(TDestNode destParent,
                                                    Func<TNode, TDestItem> mapToDestItem,
                                                    int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TDestItem>
        {
            MapCopyTo(destParent, null, mapToDestItem, maxRelativeDepth);
        }


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
