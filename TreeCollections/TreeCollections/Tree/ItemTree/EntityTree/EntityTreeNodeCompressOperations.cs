
using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class EntityTreeNode<TNode, TId, TItem>
    {
        public void CompressTo(TNode destRoot,
                               Func<TNode, bool> matchesCriteria,
                               int? maxRelativeSearchDepth = null,
                               int? maxRelativeRenderDepth = null)
        {
            MapCompressTo(destRoot, matchesCriteria, item => item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }


        public void CompressTo(TNode destRoot,
                               Func<TNode, bool> allowNext,
                               Func<TNode, bool> matchesCriteria,
                               int? maxRelativeSearchDepth = null,
                               int? maxRelativeRenderDepth = null)
        {
            MapCompressTo(destRoot, allowNext, matchesCriteria, item => item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }


        public void MapCompressTo<TDestNode>(TDestNode destRoot,
                                             Func<TNode, bool> matchesCriteria,
                                             int? maxRelativeSearchDepth = null,
                                             int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TItem>
        {
            MapCompressTo(destRoot, matchesCriteria, item => item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }


        public void MapCompressTo<TDestNode>(TDestNode destRoot,
                                             Func<TNode, bool> allowNext,
                                             Func<TNode, bool> matchesCriteria,
                                             int? maxRelativeSearchDepth = null,
                                             int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TItem>
        {
            MapCompressTo(destRoot, allowNext, matchesCriteria, item => item, maxRelativeSearchDepth, maxRelativeRenderDepth);
        }


        public void MapCompressTo<TDestNode, TDestItem>(TDestNode destRoot,
                                                        Func<TNode, bool> matchesCriteria,
                                                        Func<TItem, TDestItem> mapItem,
                                                        int? maxRelativeSearchDepth = null,
                                                        int? maxRelativeRenderDepth = null)
            where TDestNode : EntityTreeNode<TDestNode, TId, TDestItem>
        {
            if (maxRelativeRenderDepth <= 0) return;

            MapCompressTo(PreOrder(maxRelativeSearchDepth), destRoot, matchesCriteria, mapItem, maxRelativeRenderDepth);
        }


        public void MapCompressTo<TDestNode, TDestItem>(TDestNode destRoot,
                                                        Func<TNode, bool> allowNext,
                                                        Func<TNode, bool> matchesCriteria,
                                                        Func<TItem, TDestItem> mapItem,
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
                                                                Func<TItem, TDestItem> mapItem, 
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

            destRoot.Build(parent => sourceDescendantNodePool[parent.Id].Select(c => mapItem(c.Item)), maxRelativeRenderDepth);
        }
    }
}
