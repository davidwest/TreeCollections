using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public static class TreeQueryExtensions
    {
        public static IEnumerable<TNode> WhereSupports<TNode>(this IEnumerable<TNode> seq, Func<TNode, bool> satisfiesCondition) 
            where TNode : TreeNode<TNode>
        {
            return seq
                   .Where(satisfiesCondition)
                   .SelectMany(n => n.SelectPathUpward())
                   .Distinct();
        } 
    }
}
