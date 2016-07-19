
using System;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class ItemTreeNode<TNode, TItem>
    {
        public void CopyTo(TNode destParent,
                           int? maxRelativeDepth = null)
        {
            MapCopyTo(destParent, n => true, node => node.Item, maxRelativeDepth);
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
            MapCopyTo(destParent, n => true, node => node.Item, maxRelativeDepth);
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
            MapCopyTo(destParent, n => true, mapToDestItem, maxRelativeDepth);
        }


        public void MapCopyTo<TDestNode, TDestItem>(TDestNode destParent, 
                                                    Func<TNode, bool> allowNext, 
                                                    Func<TNode, TDestItem> mapToDestItem, 
                                                    int? maxRelativeDepth = null)
            where TDestNode : ItemTreeNode<TDestNode, TDestItem>
        {
            if (!allowNext(This) || maxRelativeDepth <= 0) return;

            MapCopyTo(This, destParent, allowNext, mapToDestItem, 0, maxRelativeDepth ?? int.MaxValue);
        }


        private static void MapCopyTo<TDestNode, TDestItem>(TNode sourceParent, 
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
                .Zip(destParent.Children, (sourceChild, destChild) => new { sourceChild, destChild });

            foreach (var pair in childPairs)
            {
                MapCopyTo(pair.sourceChild, pair.destChild, allowNext, mapToDestItem, curDepth, maxRelativeDepth);
            }
        }
    }
}
